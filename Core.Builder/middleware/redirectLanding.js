// Will be used eventually to gate access to the portal

export default async function ({ store, redirect, route }) {
	try {
		if (
			//Soon we'll only want to use this for agents
			//store.getters["user/role"] == "Agent" &&
			!route.fullPath.includes("landing") &&
			!route.fullPath.includes("auth") &&
			!store.getters["user/hasAcceptedTerms"]
		) {
			//redirect("/landing/agentwelcome");
		}
	} catch (err) {
		console.log(err);
	}
}
