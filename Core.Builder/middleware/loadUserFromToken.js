export default async function ({ $auth, route, $axios, redirect }) {
	try {
		if (!$auth.$state.loggedIn && !route.fullPath.includes("auth")) {
			if (route.query?.coretoken) {
				const token = await $axios.$get(
					`/auth/loginsso?coretoken=${route.query.coretoken}`
				);
				await $auth.setUserToken(token.token, token.refreshToken);
			}
		}
	} catch (err) {
		console.log(err);
	}
}
