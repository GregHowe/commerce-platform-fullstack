## Store Tests
This directory is for testing stores in isolation. 

Due to the way nuxt auto-generates store files, the tests which only use the store directly can only run out side of the `/store` directory, or they fail in the test suite.

All tests which use the store in actual components can stay in the same directory as their component.
