<template>
	<div style="height: 4px">
		<v-progress-linear
			:active="active"
			:color="color"
			:indeterminate="true"
			class="ma-0"
			height="4"
		>
			Loading...
		</v-progress-linear>
	</div>
</template>
<script>
export default {
	props: {
		active: {
			type: Boolean,
			default: false,
		},
		color: {
			type: String,
			default: "primary",
		},
	},
};
</script>
<style lang="scss">
.v-toolbar__extension {
	top: -2px;
	padding: 0px !important;
}
.v-progress-linear {
	transform: scale(1, -1);
}
.v-progress-linear__content {
	display: none;
}
</style>
