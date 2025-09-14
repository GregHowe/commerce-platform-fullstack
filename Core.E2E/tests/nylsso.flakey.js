const { test, expect } = require("@playwright/test");

test.describe("NYL SSO", () => {
  test("should allow me to login as a NYL user", async ({ page }) => {
    await page.goto("/");
    await expect(page.locator("#loginbtn")).toBeVisible();
    await page.screenshot({ path: "./playwright-screenshots/nyl-login.png" });
    await page.waitForTimeout(1000);
    await page.click(
      ".v-card__text > .container > .row > .mx-4:nth-child(1) > .v-btn__content"
    );

    //await page.fill("#username", "AGTAH5R");
    await page.fill("#username", "AGTKLC1");
    await page.fill("#password", "QAmar2023");
    await page.screenshot({
      path: "./playwright-screenshots/nyl-pingfederated.png",
    });
    await page.click("#btn_submit");
    //await page.waitForTimeout(8000);
    //await expect(page.locator("#logoutbtn")).toBeVisible();
    await expect(page.locator("h3:has-text('Welcome!')")).toBeVisible();
    await page.screenshot({ path: "./playwright-screenshots/nyl-home.png" });
  });
});
