<template>
	<div class="block__body">
		<div class="block__section_content">File</div>
	</div>
</template>

<script>
export default {
	name: "CoreBlockFile",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
};
</script>

<style lang="scss">
.block_file {
	.block__body {
		.block__section_content {
			position: relative;
			width: 100%;
			height: 100%;
		}
	}
}
</style>
