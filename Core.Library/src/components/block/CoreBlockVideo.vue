<template>
	<div class="block__body">
		<div class="block__section_content">
			<iframe
				class="block__section_content--player"
				type="text/html"
				:src="settings.src"
			>
			</iframe>
		</div>
	</div>
</template>

<script>
export default {
	name: "CoreBlockVideo",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
};
</script>
<style lang="scss">
.block_video {
	.block__body {
		.block__section_content {
			background-color: #606060;
			position: relative;
			overflow: hidden;
			width: 100%;
			padding-top: 56.25%; /* 16:9 Aspect Ratio (divide 9 by 16 = 0.5625) */
			& .block__section_content--player {
				position: absolute;
				top: 0;
				left: 0;
				bottom: 0;
				right: 0;
				width: 100%;
				height: 100%;
			}
		}
	}
}
</style>
