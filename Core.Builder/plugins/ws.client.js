let ws = {};
const axios = require("axios");
let wsKeepAlive = {};

// This proves out we can send out notifications, but needs to be refined

const publishEvents = {
	publish: {
		type: "PUBLISH",
		title: "Started",
		description: "Your build has started.",
		color: "info",
		icon: "mdi-close-network-outline",
	},
	publish_failed: {
		type: "PUBLISH",
		title: "Error",
		description: "Your build has failed.",
		color: "warning",
		icon: "mdi-close-network-outline",
	},
	publish_success: {
		type: "PUBLISH",
		title: "Complete",
		description: "Your site is now live!",
		color: "green",
		icon: "mdi-check-bold",
	},
};

const isOpen = (_ws) => {
	return _ws.readyState === _ws.OPEN;
};

export default async ({ store, $axiosNode }, inject) => {
	//store.dispatch("buildQueue/GET_PIPELINE_STATUS");
	try {
		const res = await $axiosNode.get(
			`negotiate?userId=${store.getters["user/fullName"]}`,
			{
				credentials: "include",
			}
		);

		if (!res.data.url) return true;

		// connect
		ws = new WebSocket(res.data.url);
		ws.onopen = () => console.log("CoreLink established.");
		ws.onclose = () => console.log("CoreLink closed.");
		ws.onmessage = (event) => {
			try {
				const data = JSON.parse(event.data);
				console.log("CoreLink message received", data);
				store.dispatch("interface/SET_NOTIFICATION", {
					...publishEvents[data.action],
					link: `https://f92core-nylwebsites.azureedge.net/${data.brandId}/websites/${data.siteId}/latest/`,
					details: `Brand ${data.brandId} Site ${data.siteId}`,
				});
			} catch (err) {
				console.log(err);
			}
		};
		/* Might not need this to keep connection alive
	wsKeepAlive = setInterval(
		() => (isOpen(ws) ? "" : (ws = new WebSocket(res.url))),
		1000
	);
	*/
	} catch (err) {
		console.error("Websocket could not connect", err);
	}
	inject("ws", ws);
};
