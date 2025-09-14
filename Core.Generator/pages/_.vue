<template>
	<div v-if="page">
		<CoreBlock
			v-for="blockSettings in page.blocks"
			:key="blockSettings.id"
			:settings="blockSettings"
		/>
	</div>
</template>

<script>
export default {
	async asyncData({ store, params }) {
		let page = null;
		if (params.pathMatch === "") {
			// is this the home route?
			page = { ...store.getters.getHomePage };
		} else {
			if (!params.pathMatch) {
				return;
			}
			page = {
				...store.getters.getPageByHandle(
					params.pathMatch.slice(
						params.pathMatch.lastIndexOf("/") + 1
					)
				),
			};
		}
		page.blocks = page?.blocks || [];
		if (typeof page.blocks === "string") {
			page.blocks = JSON.parse(page.blocks);
		}
		const themes = {
			color: store.getters.getThemeByKey("color"),
			font: store.getters.getThemeByKey("font"),
		};
		return {
			page,
			themes,
		};
	},
	head() {
		let cssText = [this.themes.color, this.themes.font].join("\n");
		return {
			style: [
				{
					cssText,
					type: "text/css",
				},
			],
		};
	},
};
</script>
