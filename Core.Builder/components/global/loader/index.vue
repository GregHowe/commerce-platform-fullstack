<template>
	<transition
		name="quickfade"
		mode="in-out"
	>
		<div
			v-show="itemsBeingLoaded.length"
			class="globalloader px-4 py-2"
		>
			<div
				v-for="item in itemsBeingLoaded"
				:key="item"
				class="white--text loaderitem"
			>
				<span class="loaderitemtext"
					><v-progress-circular
						:size="16"
						:width="3"
						color="white"
						indeterminate
						class="mr-2 d-inline-block text-caption"
						style="position: relative; top: -2px"
					/>{{ item }}</span
				>
			</div>
		</div>
	</transition>
</template>
<script>
import { mapState } from "vuex";
export default {
	computed: {
		...mapState({
			itemsBeingLoaded: (state) =>
				state.interface.busy.loading.concat(
					state.interface.busy.saving
				),
		}),
	},
};
</script>
<style
	lang="scss"
	scoped
>
.globalloader {
	background: var(--v-primary-base);
	position: fixed;
	bottom: 20px;
	right: 20px;
	border-radius: 4px;
	text-overflow: ellipsis;
}
.loaderitem,
.loaderitemtext {
	text-overflow: ellipsis;
	max-width: 100%;
}
</style>
