<template>
	<div class="block__body">
		<figure class="block__section_content">
			<img
				:src="imageSrc"
				:alt="imageAlt"
				:style="{
					height: imageHeight,
					width: imageWidth,
				}"
			/>
		</figure>
	</div>
</template>

<script>
export default {
	name: "CoreBlockImage",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		imageAlt() {
			return this.settings.alt || null;
		},
		imageSrc() {
			return this.settings.src || null;
		},
		imageHeight() {
			return this.settings.height || null;
		},
		imageWidth() {
			return this.settings.width || null;
		},
	},
};
</script>

<style lang="scss">
.block_image {
	.block__body {
		height: 100%;
		.block__section_content {
			position: relative;
			width: 100%;
			height: auto;
			img {
				display: block;
				height: auto;
				min-width: 32px;
				min-height: 32px;
				max-width: 100%;
				margin: auto;
			}
		}
	}
	&.block_image_cover {
		.block__body {
			.block__section_content {
				height: 100%;
				img {
					width: 100%;
					height: 100%;
					object-fit: cover;
				}
			}
		}
	}
	&.block_image_contain {
		.block__body {
			.block__section_content {
				height: 100%;
				img {
					height: 100%;
					object-fit: contain;
				}
			}
		}
	}
}
</style>
