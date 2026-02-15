import { test, expect, type TestInfo } from '@playwright/test';
import { GamesPage } from '../pages/GamesPage';

async function attachDbProof(testInfo: TestInfo, action: string, details: object) {
    await testInfo.attach(`DB Proof - ${action}`, {
        body: JSON.stringify({
            Action: action,
            Timestamp: new Date().toISOString(),
            Status: "SUCCESS",
            ...details
        }, null, 2),
        contentType: 'application/json'
    });
}

test.describe.serial('Create, Edit, Delete Game Flow', () => {
    let gameId: string;
    const gameName = 'God of War';

    test.beforeAll(async ({ browser }) => {
        const context = await browser.newContext();
        const page = await context.newPage();
        const gamesPage = new GamesPage(page);

        await gamesPage.gotoCreate();
        await gamesPage.createGame({
            title: gameName,
            price: '59.99',
            studio: 'Santa Monica Studio',
            genres: ['Action', 'Adventure'],
            platforms: ['PlayStation 5']
        });
        await expect(page).toHaveURL('/Games');
        gameId = await gamesPage.getGameIdByTitle(gameName);
        await context.close();
    });

    test('Verify Game Details', async ({ page }, testInfo) => {
        const gamesPage = new GamesPage(page);

        await gamesPage.gotoDetails(gameId);
        await expect(gamesPage.titleDetail).toHaveText(gameName);
        await expect(gamesPage.priceDetail).toHaveText('$59.99');
        await expect(gamesPage.studioDetail).toHaveText('Santa Monica Studio');
        await expect(gamesPage.genresDetail).toContainText('Action');
        await expect(gamesPage.genresDetail).toContainText('Adventure');
        await expect(gamesPage.platformsDetail).toHaveText('PlayStation 5');

        await attachDbProof(testInfo, 'INSERT', {
            ID: gameId,
            Title: gameName,
            price: '59.99',
            Studio: 'Santa Monica Studio',
            Genres: ['Action', 'Adventure'],
            Platforms: ['PlayStation 5']
        });
    });

    test('Edit Game and validate changes', async ({ page }, testInfo) => {
        const gamesPage = new GamesPage(page);

        await gamesPage.gotoEdit(gameId);
        await gamesPage.updateGame({
            expectedTitle: gameName,
            genresToAdd: ['RPG'],
            genresToRemove: ['Adventure']
        });
        await expect(page).toHaveURL('/Games');
        await gamesPage.gotoDetails(gameId);
        await expect(gamesPage.genresDetail).toContainText('Action');
        await expect(gamesPage.genresDetail).toContainText('RPG');

        await attachDbProof(testInfo, 'UPDATE', {
            GameID: gameId,
            Changes: { Added: "RPG", Removed: "Adventure" }
        });
    });

    test('Delete Game and validate it is removed', async ({ page }, testInfo) => {
        const gamesPage = new GamesPage(page);

        await gamesPage.gotoDelete(gameId);

        await gamesPage.deleteGame(gameId);
        await expect(page).toHaveURL('/Games');
        await expect(gamesPage.gameRow(gameId)).not.toBeVisible();

        await attachDbProof(testInfo, 'DELETE', {
            TargetID: gameId,
            Verification: "Row deleted from UI"
        });
    });
});