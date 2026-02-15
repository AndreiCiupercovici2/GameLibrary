import { test, expect } from '@playwright/test';
import { GamesPage } from '../pages/GamesPage';

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

    test('Verify Game Details', async ({ page }) => {
        const gamesPage = new GamesPage(page);

        await gamesPage.gotoDetails(gameId);
        await expect(gamesPage.titleDetail).toHaveText(gameName);
        await expect(gamesPage.priceDetail).toHaveText('$59.99');
        await expect(gamesPage.studioDetail).toHaveText('Santa Monica Studio');
        await expect(gamesPage.genresDetail).toContainText('Action');
        await expect(gamesPage.genresDetail).toContainText('Adventure');
        await expect(gamesPage.platformsDetail).toHaveText('PlayStation 5');
    });

    test('Edit Game and validate changes', async ({ page }) => {
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
    });

    test('Delete Game and validate it is removed', async ({ page }) => {
        const gamesPage = new GamesPage(page);

        await gamesPage.gotoDelete(gameId);

        await gamesPage.deleteGame(gameId);
        await expect(page).toHaveURL('/Games');
        await expect(gamesPage.gameRow(gameId)).not.toBeVisible();
    });
});