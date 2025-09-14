<template>
	<div
		class="block__body"
		:style="{
			...background,
		}"
	>
		<CoreBlockBreadcrumbs
			v-if="settings.showBreadcrumb"
			:settings="settings"
		/>
		<component
			:is="childBlockComponent"
			:index="0"
			:is-editable="isEditable"
			:is-first-child="true"
			:is-last-child="true"
			:settings="getSettings()"
		/>
		<CoreBlock
			v-if="settings.showKeepScrolling"
			:settings="{
				type: 'button',
				label: 'Keep Scrolling',
				variants: {
					scroll: true,
				},
				event: scrollDown,
			}"
		/>
	</div>
</template>

<script>
export default {
	name: "CoreBlockHero",
	props: {
		index: {
			type: Number,
			default: 0,
		},
		nestedLevel: {
			type: Number,
			default: 0,
		},
		settings: {
			type: Object,
			required: true,
		},
		isEditable: {
			type: Boolean,
			default: false,
		},
	},
	computed: {
		background() {
			if (this.settings?.backgroundMediaSrc) {
				return {
					"background-image": `url(${this.settings.backgroundMediaSrc})`,
					"background-position": "center",
					"background-repeat": "no-repeat",
				};
			}
			return null;
		},
		childBlockComponent() {
			if (this.isEditable) {
				return "ForgeControlBlock";
			}
			return "CoreBlock";
		},
		blockId() {
			return this.settings.id;
		},
		variants() {
			return this.settings.variants || {};
		},
		imageAlign() {
			return (this.variants["image-align"] || "left").replace(
				"image-",
				""
			);
		},
		textAlign() {
			return (this.variants["text-align"] || "left").replace("text-", "");
		},
		verticalAlign() {
			return this.variants["vertical-align"] || null;
		},
	},
	methods: {
		getSettings() {
			const response = {
				type: "columns",
				"column-count": 2,
				variants: {
					layout: "grid",
					"vertical-alignment": this.verticalAlign,
					"image-alignment": this.imageAlign,
					"padding-top": "pt-small",
					"bg-color": "transparent",
					layout: "contained",
				},
				locked: true,
				blocks: [],
			};
			const imageBlock = {
				type: "rows",
				blocks: [],
				variants: {
					"bg-color": "transparent",
				},
			};
			if (this.settings.sideMediaSrc) {
				imageBlock.blocks.push({
					id: `i1_${this.settings.id}`,
					type: "media",
					acceptTypes: ["video", "image"],
					variants: {
						sizing: "contain",
						"bg-color": "transparent",
					},
					src: this.settings.sideMediaSrc,
					width: "100%",
					height: "auto",
				});
			}
			response.blocks.push(imageBlock);
			const contentBlock = {
				id: `c1_${this.settings.id}`,
				type: "rows",
				variants: {
					"padding-horizontal": "px-medium",
					"bg-color": "transparent",
				},
				blocks: [
					{
						type: "text",
						variants: {
							size: "overline",
							align: this.textAlign,
						},
						text: this.settings.overline,
					},
					{
						type: "text",
						variants: {
							size: "headline",
							align: this.textAlign,
							weight: "bold",
						},
						text: this.settings.headline,
					},
					{
						type: "text",
						variants: {
							size: "subline",
							align: this.textAlign,
							weight: "bold",
						},
						text: this.settings.subline,
					},
					{
						type: "rich-text",
						variants: {
							align: this.textAlign,
						},
						richText: this.settings.bodyText,
					},
				],
			};
			const buttonContainer = this.getButtonContainer();
			if (buttonContainer) {
				contentBlock.blocks.push(buttonContainer);
			}
			response.blocks.push(contentBlock);
			return response;
		},
		getButtonContainer() {
			let response = null;
			if (
				this.settings["button-primary-url"] ||
				this.settings["button-secondary-url"] ||
				this.settings["button-tertiary-url"]
			) {
				response = {
					id: `c2_${this.settings.id}`,
					type: "container",
					variants: {
						layout: "columns",
						"bg-color": "transparent",
					},
					locked: false,
					allowTypes: ["button"],
					blocks: [],
				};
				if (this.settings["button-primary-url"]) {
					response.blocks.push({
						id: `b1_${this.settings.id}`,
						type: "button",
						variants: {
							style: "primary",
						},
						label:
							this.settings["button-primary-label"] ||
							"Primary Button Label",
						url: this.settings["button-primary-url"],
					});
				}
				if (this.settings["button-secondary-url"]) {
					response.blocks.push({
						id: `b2_${this.settings.id}`,
						type: "button",
						variants: {
							style: "secondary",
						},
						label:
							this.settings["button-secondary-label"] ||
							"Secondary Button Label",
						url: this.settings["button-secondary-url"],
					});
				}
				if (this.settings["button-tertiary-url"]) {
					response.blocks.push({
						id: `b3_${this.settings.id}`,
						type: "button",
						variants: {
							style: "tertiary",
						},
						label:
							this.settings["button-tertiary-label"] ||
							"Tertiary Button Label",
					});
				}
			}
			return response;
		},
		scrollDown() {
			if (process.client) {
				window.scroll({ top: 900, behavior: "smooth" });
			}
		},
	},
};
</script>

<style lang="scss">
.block_hero {
	.block__body {
		& > .block__section_main {
			flex-direction: row;
		}
	}
}
</style>
