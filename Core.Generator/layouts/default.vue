<template>
	<div class="page">
		<component
			:is="'style'"
			v-if="themes.color"
		>
			{{ themes.color }}
		</component>
		<component
			:is="'style'"
			v-if="themes.font"
		>
			{{ themes.font }}
		</component>
		<component
			:is="'style'"
			v-if="customStyles"
		>
			{{ customStyles }}
		</component>
		<div class="page__body">
			<CoreBlockMainNav :site="site" />
			<!-- <div class="page__blocks"> -->
				<Nuxt class="page__blocks" />
			<!-- </div> -->
			<CoreBlockFooter :site="site" />
		</div>
	</div>
</template>

<script>
export default {
	computed: {
		themes() {
			return this.site?.themes;
		},
		site() {
			return this.$store.state.site;
		},
		customStyles() {
			return this.site?.style || null;
		},
	},
};
</script>
