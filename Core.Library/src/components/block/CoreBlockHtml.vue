<template>
	<component
		:is="blockTag"
		v-if="blockContent"
		class="block__body"
		v-html="blockContent"
	></component>
</template>

<script>
export default {
	name: "CoreBlockText",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		blockContent() {
			return this.settings?.html || null;
		},
		blockTag() {
			return this.settings?.tag || "div";
		},
	},
};
</script>

<style lang="scss">
</style>
