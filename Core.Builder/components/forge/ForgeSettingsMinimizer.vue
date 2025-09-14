<template>
	<div
		v-if="isSettingsMaximized"
		class="forge-settings-minimizer"
		@click="minimize"
	>
		<a class="closer">x</a>
	</div>
</template>
<script>
import { mapMutations, mapState } from "vuex";
export default {
	name: "ForgeSettingsMinimizer",
	computed: {
		...mapState("interface", ["isSettingsMaximized"]),
	},
	methods: {
		...mapMutations("interface", ["setMaximizeSettings"]),
		minimize() {
			this.setMaximizeSettings(false);
		},
	},
};
</script>
<style lang="scss">
.forge-settings-minimizer {
	background: rgba(0, 0, 0, 0.6);
	position: fixed;
	top: 0;
	right: 0;
	bottom: 0;
	left: 0;
	z-index: 5;
	cursor: pointer;
	.closer {
		pointer-events: none;
		color: white;
		position: absolute;
		top: 1rem;
		right: 1160px;
		font-size: 3rem;
	}
}
</style>
