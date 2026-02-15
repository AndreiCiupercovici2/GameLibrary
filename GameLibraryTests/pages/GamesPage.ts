import {type Page, type Locator, expect} from '@playwright/test';
import {BasePage} from './BasePage';

export class GamesPage extends BasePage {
    readonly createNewBtn: Locator;
    readonly editBtn: (id: string) => Locator;
    readonly deleteBtn: Locator;
    readonly detailsBtn: (id: string) => Locator;
    readonly backToListBtn: Locator;
    readonly gamesTable: Locator;
    readonly gameRow: (id: string) => Locator;
    readonly genreCell: (id: string) => Locator;
    readonly platformCell: (id: string) => Locator;

    readonly titleInput: Locator;
    readonly priceInput: Locator;
    readonly studioSelect: Locator;
    readonly submitBtn: Locator;

    readonly titleDetail: Locator;
    readonly priceDetail: Locator;
    readonly studioDetail: Locator;
    readonly genresDetail: Locator;
    readonly platformsDetail: Locator;


    constructor(page: Page) {
        super(page);
        this.createNewBtn = page.getByTestId('create-game-btn');
        this.backToListBtn = page.getByTestId('back-to-list-btn');
        this.gamesTable = page.getByTestId('games-table');
        this.titleInput = page.getByTestId('game-title-input');
        this.priceInput = page.getByTestId('game-price-input');
        this.studioSelect = page.getByTestId('studio-select');
        this.submitBtn = page.getByTestId('submit-btn');
        this.gameRow = (id: string) => page.getByTestId(`game-row-${id}`);
        this.editBtn = (id: string) => page.getByTestId(`edit-game-btn-${id}`);
        this.deleteBtn = page.getByTestId('confirm-delete-btn');
        this.detailsBtn = (id: string) => page.getByTestId(`details-game-btn-${id}`);
        this.genreCell = (id: string) => page.getByTestId(`game-genres-${id}`);
        this.platformCell = (id: string) => page.getByTestId(`game-platforms-${id}`);
        this.titleDetail = page.getByTestId('game-title-value');
        this.priceDetail = page.getByTestId('game-price-value');
        this.studioDetail = page.getByTestId('game-studio-value');
        this.genresDetail = page.getByTestId('game-genres-list');
        this.platformsDetail = page.getByTestId('game-platforms-list');
    }

    async gotoCreate() {
        await this.page.goto('/games/create');
        await expect(this.page).toHaveURL('/games/create');
    }

    async gotoEdit(id: string) {
        await this.page.goto(`/games/edit/?id=${id}`);
        await expect(this.page).toHaveURL(`/games/edit/?id=${id}`);
    }

    async gotoDetails(id: string) {
        await this.page.goto(`/games/details/?id=${id}`);
        await expect(this.page).toHaveURL(`/games/details/?id=${id}`);
    }

    async gotoDelete(id: string) {
        await this.page.goto(`/games/delete/?id=${id}`);
        await expect(this.page).toHaveURL(`/games/delete/?id=${id}`);
    }

    async toggleGenres(genres: string[], action: 'check' | 'uncheck') {
        for (const genre of genres) {
            const checkbox = this.page.getByTestId(`genre-checkbox-${genre}`);
            if (action === 'check') {
                await checkbox.check();
                await expect(checkbox).toBeChecked();
            } else {
                await checkbox.uncheck();
                await expect(checkbox).not.toBeChecked();
            }
        }
    }

    async togglePlatforms(platforms: string[], action: 'check' | 'uncheck') {
        for (const platform of platforms) {
            const checkbox = this.page.getByTestId(`platform-checkbox-${platform}`);
            if (action === 'check') {
                await checkbox.check();
                await expect(checkbox).toBeChecked();
            } else {
                await checkbox.uncheck();
                await expect(checkbox).not.toBeChecked();
            }
        }   
    }

    async createGame(data: {title: string, price: string, studio: string, genres?: string[], platforms?: string[]}) {
        await this.titleInput.fill(data.title);
        await this.priceInput.fill(data.price);
        await this.studioSelect.selectOption({label: data.studio});
        if (data.genres) {
            await this.toggleGenres(data.genres, 'check');
        }
        if (data.platforms) {
            await this.togglePlatforms(data.platforms, 'check');
        }
        await this.submitBtn.click();
    }

    async updateGame(data: {
        expectedTitle: string,
        newStudio?: string,
        genresToAdd?: string[],
        genresToRemove?: string[]
    }) {
        await expect(this.titleInput).not.toBeEditable();
        await expect(this.titleInput).toHaveValue(data.expectedTitle);

        if (data.newStudio) {
            await this.studioSelect.selectOption({label: data.newStudio});
        }

        if (data.genresToAdd) {
            await this.toggleGenres(data.genresToAdd, 'check');
        }

        if (data.genresToRemove) {
            await this.toggleGenres(data.genresToRemove, 'uncheck');
        }

        await this.submitBtn.click();
    }

    async getGameIdByTitle(title: string): Promise<string> {
        const row = this.gamesTable.getByRole('row', {name: title});
        await row.waitFor({ state: 'visible' });
        const testIdAttribute = await row.getAttribute('data-testid');
        if (!testIdAttribute) {
            throw new Error(`Game with title "${title}" not found in the table.`);
        }
        const id = testIdAttribute.split('-').pop();
        if (!id) {
            throw new Error(`Could not extract game ID from test ID attribute: ${testIdAttribute}`);
        }
        return id;
    }

    async deleteGame(id: string) {
        await this.deleteBtn.click();
    }
}