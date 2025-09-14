export const state = () => ({
	queue: [],
	justPublished: false, // Pipeline doesn't process immediately, so set to true right after a publish
});

let interval;

export const getters = {
	isQueued: (state, getters, rootState) =>
		state.queue.find(
			(q) =>
				q.templateParameters.CLOUDCMS_SITEID ===
				rootState.site.workingSite.id
		),
	placeInLine: (state, getters, rootState) => {
		return (
			state.queue.findIndex(
				(q) =>
					q.templateParameters.CLOUDCMS_SITEID ===
					rootState.site.workingSite.id
			) + 1
		);
	},
	totalInQueue: (state) => state.queue.length,
	isPublishing: (state, getters) =>
		getters.isQueued && getters.placeInLine === 1,
	timeEstimate: (state, getters) => ` ${getters.placeInLine * 7} minutes`,
};

export const actions = {
	async GET_PIPELINE_STATUS({ commit, dispatch }) {
		/*
		console.log("getting pipeline status");
		// needs to be put into the dotnet backend
		const response = await this.$axios.$get(`/pipeline/queue`);
		commit("setQueue", response?.queue || []);
		commit("setJustPublished", false)
		*/
	},
};

export const mutations = {
	setQueue(state, queue) {
		state.queue = queue;
	},
	setJustPublished(state, status) {
		state.justPublished = status;
	},
};
