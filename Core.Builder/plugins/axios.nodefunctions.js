export default async function ({ $axios, $config }, inject) {
	const axiosNode = $axios.create({
		baseURL: $config.API_NODE_FUNCTIONS,
	});

	axiosNode.defaults.params = {
		code: $config.API_NODE_CODE,
	};
	inject("axiosNode", axiosNode);
}
