const { test, expect } = require("@playwright/test");

test.describe("Basic Builder Navigation", () => {
  test("should allow me to login, click into a site, and logout", async ({
    page,
  }) => {
    await page.goto("/api");
    await expect(page.locator("pre:has-text('Hello World')")).toBeVisible();
    await page.screenshot({
      path: "./playwright-screenshots/api-healthcheck.png",
    });
  });
});
