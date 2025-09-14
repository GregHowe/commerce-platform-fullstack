const { test, expect } = require("@playwright/test");

test.describe("Basic Builder Navigation", () => {
  test("should allow me to login, click into a site, and logout", async ({
    page,
  }) => {
    await page.goto("/");

    await page.screenshot({ path: "./playwright-screenshots/basic-login.png" });
    await page.fill("#login-username", "n.elliott@fusion92.com");
    await page.fill("#login-password", "d991b153-2bc4-4a7b-bbfd-078532e9ee2a");
    await page.screenshot({
      path: "./playwright-screenshots/basic-readytologin.png",
    });
    //await expect(page.locator("#loginbtn")).toBeVisible();
    await page.locator("#loginbtn").click();
    await page.screenshot({
      path: "./playwright-screenshots/basic-loggingin.png",
    });
    await expect(page.locator("h1")).toBeVisible();

    await page.screenshot({
      path: "./playwright-screenshots/basic-loggedin.png",
    });
    await page.keyboard.press("Escape");
    await page.locator("a:has-text('Links Test')").click();

    await page.screenshot({
      path: "./playwright-screenshots/basic-clickedsite.png",
    });
    await expect(page.locator("h1:has-text('Links Test')")).toBeVisible();
    await page.locator("#logoutbtn").click();
    await page.screenshot({
      path: "./playwright-screenshots/basic-loggingout.png",
    });
    await expect(page.locator("#loginbtn")).toBeVisible();
    await page.screenshot({
      path: "./playwright-screenshots/basic-loggedout.png",
    });
  });
});
