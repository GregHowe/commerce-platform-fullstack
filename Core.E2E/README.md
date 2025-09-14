This is designed to run after the builder is pushed to staging. It will act as a smoke test to ensure what is in the main branch passes muster. I've added a basic test for now, but we will want more. Ideally one for each important user flow.

If you want to run these tests locally, navigate to /Core.E2E and create an .env file.
Set its contents to TEST_BASE_URL=http://localhost:3000. Run npm install if you haven't. Then run npx playwright test.
