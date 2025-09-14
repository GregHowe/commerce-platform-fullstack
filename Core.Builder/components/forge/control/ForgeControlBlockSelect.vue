<template>
	<div class="forge__control">
		<div
			class="forge__control_select"
			@click="
				() => {
					$emit('select');
				}
			"
		/>
	</div>
</template>
<script>
export default {
	props: {
		blockType: {
			type: String,
			default: null,
		},
		isActive: {
			type: Boolean,
			default: false,
		},
	},
};
</script>

<style lang="scss">
.forge__control_select {
	pointer-events: all;
	position: absolute;
	top: 0;
	right: 0;
	bottom: 0;
	left: 0;
	opacity: 0.2;
}
</style>
