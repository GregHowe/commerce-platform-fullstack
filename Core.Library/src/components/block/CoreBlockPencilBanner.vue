<template>
	<div
		v-if="!bannerIsClosed"
		class="block__body"
		role="banner"
	>
		<div class="block__body-container">
			<div
				class="block__body-container_content"
				v-html="bodyCopy"
			></div>
			<CoreBlock
				v-if="label"
				class="block__body-container_button"
				:settings="{
					type: 'button',
					label,
					newTab: settings.newTab,
					url,
					icon: 'right-arrow',
					variants: {
						style: 'text',
						'icon-placement': 'icon-right',
					},
				}"
			/>
		</div>
		<CoreIcon
			v-if="settings.closeButton && !bannerIsClosed"
			icon="close"
			@click="bannerIsClosed = true"
		/>
	</div>
</template>

<script>
import { renderData } from "@libraryHelpers/dataComponents.js";

export default {
	name: "CoreBlockPencilBanner",
	props: {
		settings: {
			type: Object,
			required: true,
		},
		dataSite: {
			type: Object,
			default: () => ({}),
		},
	},
	data() {
		return {
			bannerIsClosed: false,
		};
	},
	computed: {
		bodyCopy() {
			return renderData(this.settings.bodyCopy, this.dataSite);
		},
		label() {
			return renderData(this.settings.buttonText, this.dataSite);
		},
		url() {
			return renderData(this.settings.buttonLink, this.dataSite);
		},
	},
};
</script>

<style lang="scss">
.block_pencil-banner {
	.block {
		width: unset;
	}
	.block__body-container {
		justify-content: inherit;
	}
	.block__body {
		display: flex;
		width: 100%;
		align-items: center;
		justify-content: space-between;
		padding: 0.3rem;
		background-color: var(--_core__pencil-banner_background);
		&-container {
			width: 100%;
			display: flex;
			align-items: center;
			flex-wrap: wrap;
			&_content {
				color: var(--_core__pencil-banner_color);
				font-family: var(--_core__pencil-banner_font-family);
				font-size: var(--_core__pencil-banner_font-size);
			}
			&_button {
				.block__body {
					margin: 0;
				}
			}
		}
	}
}
</style>
