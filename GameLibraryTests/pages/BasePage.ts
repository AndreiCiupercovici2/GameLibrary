import {type Page, type Locator, expect} from '@playwright/test';

export class BasePage {
  readonly page: Page;

  readonly navBrand: Locator;
  readonly navGames: Locator;
  readonly navStudios: Locator;
  readonly navGenres: Locator;
  readonly navPlatforms: Locator;

  readonly languageSelector: Locator;
  readonly privacyLink: Locator;

  constructor(page: Page) {
    this.page = page;

    this.navBrand = page.getByTestId('index-page-btn');
    this.navGames = page.getByTestId('games-page-btn');
    this.navStudios = page.getByTestId('studios-page-btn');
    this.navGenres = page.getByTestId('genres-page-btn');
    this.navPlatforms = page.getByTestId('platforms-page-btn');

    this.languageSelector = page.getByTestId('language-selector');
    this.privacyLink = page.getByTestId('privacy-link');
  }

  async navigateToHomePage() {
    await this.navBrand.click();
    await expect(this.page).toHaveURL('/');
  }

  async navigateToGamesPage() {
    await this.navGames.click();
    await expect(this.page).toHaveURL('/Games');
  }

  async navigateToStudiosPage() {
    await this.navStudios.click();
    await expect(this.page).toHaveURL('/Studios');
  }
  async navigateToGenresPage() {
    await this.navGenres.click();
    await expect(this.page).toHaveURL('/Genres');
  }
  
  async navigateToPlatformsPage() {
    await this.navPlatforms.click();
    await expect(this.page).toHaveURL('/Platforms');
  }

  async selectLanguage(language: string) {
    await this.languageSelector.selectOption(language);
    await expect(this.languageSelector).toHaveValue(language);
  }

  async clickPrivacyLink() {
    await this.privacyLink.click();
    await expect(this.page).toHaveURL('/privacy');
  }

  async verifyNavigationBar() {
    await expect(this.navBrand).toBeVisible();
    await expect(this.navGames).toBeVisible();
    await expect(this.navStudios).toBeVisible();
    await expect(this.navGenres).toBeVisible();
    await expect(this.navPlatforms).toBeVisible();
  }

  async verifyLanguageSelector() {
    await expect(this.languageSelector).toBeVisible();
    const options = await this.languageSelector.locator('option').allTextContents();
    expect(options).toContain('English');
    expect(options).toContain('Spanish');
  }

  async verifyPrivacyLink() {
    await expect(this.privacyLink).toBeVisible();
    await expect(this.privacyLink).toHaveAttribute('href', '/privacy');
  }

  async verifyPageTitle(expectedTitle: string) {
    await expect(this.page).toHaveTitle(expectedTitle);
  }
}