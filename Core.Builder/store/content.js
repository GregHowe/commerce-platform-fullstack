export const state = () => ({});

export const getters = {};

export const actions = {
	async uploadFile({ rootState }, upload) {
		if (upload.file.name) {
			const siteId = rootState.site.selectedSiteId;
			const targetDirectory = siteId ? `/sites/${siteId}` : "/library";
			const formData = new FormData();
			formData.append("file", upload.file);
			formData.append("name", upload.file.name);
			formData.append("size", upload.file.size);
			formData.append("type", upload.file.type);
			const response = await this.$axios.$post(
				`${targetDirectory}/media/upload`,
				formData,
				{
					headers: {
						"Content-Type": "multipart/form-data",
					},
				}
			);
			const uri = response?.blob?.uri || response?.settings?.Url;
			return uri || null; // server returning a BLOB with details about the location of hte uploaded file
		}
	},
};

export const mutations = {};
