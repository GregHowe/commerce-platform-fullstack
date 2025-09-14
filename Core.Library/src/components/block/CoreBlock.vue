<template>
	<component
		:is="blockTag"
		:id="blockId"
		:target="linkTarget"
		:class="blockClasses"
		:href="settings.linkInternal ? '' : linkUrl"
		:to="settings.linkInternal ? '' : linkUrl"
	>
		<component
			:is="blockComponent"
			:settings="settings"
			:nested-level="nestedLevel"
			:index="index"
			:is-editable="isEditable"
			:data-site="dataSite"
			@select="select"
			@insert="insert"
		/>
		<slot name="controls" />
		<component
			:is="'style'"
			v-if="blockStyle"
		>
			{{ blockStyle }}
		</component>
	</component>
</template>

<script>
import { sitePath } from "@libraryHelpers/dataComponents.js";

export default {
	name: "CoreBlock",
	props: {
		index: {
			type: Number,
			default: 0,
		},
		isEditable: {
			type: Boolean,
			default: false,
		},
		nestedLevel: {
			type: Number,
			default: 0,
		},
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		dataSite() {
			// this is the site object used in both the generator and the builder
			return sitePath(this.$store);
		},

		blockTag() {
			if (this.settings?.linkUrl) {
				if (this.settings?.linkInternal) {
					return "NuxtLink";
				}
				return "a";
			}
			return this.settings?.linkUrl ? "a" : "div";
		},
		linkUrl() {
			return this.settings?.linkUrl;
		},
		linkTarget() {
			return this.settings.linkInternal ? "_self" : "_blank";
		},
		blockComponent() {
			switch (this.settings.type) {
				case "button":
					return "CoreBlockButton";
				case "breadcrumbs":
					return "CoreBlockBreadcrumbs";
				case "contact-card":
					return "CoreBlockContactCard";
				case "contact-hero":
					return "CoreBlockContactHero";
				case "columns":
					return "CoreBlockColumns";
				case "container":
				case "section":
				case "accordion":
				case "toggle":
				case "tabs":
					return "CoreBlockContainer";
				case "content-card":
					return "CoreBlockContentCard";
				case "divider":
					return "CoreBlockDivider";
				case "file":
					return "CoreBlockFile";
				case "grid":
					return "CoreBlockGrid";
				case "hero":
					return "CoreBlockHero";
				case "html": 
					return "CoreBlockHtml";
				case "icon":
					return "CoreBlockIcon";
				case "image":
					return "CoreBlockImage";
				case "media":
					return "CoreBlockMedia";
				case "rows":
					return "CoreBlockRows";
				case "social-links":
					return "CoreBlockSocialLinks";
				case "text":
					return "CoreBlockText";
				case "video":
					return "CoreBlockVideo";
				case "pencil-banner":
					return "CoreBlockPencilBanner";
				case "rich-text":
					return "CoreBlockRichText";
				case "form":
					return "CoreBlockForm";
				case "calendly":
					return "CoreBlockCalendly";
				case "map":
					return "CoreBlockMap";
			}
			return "CoreBlockUnknown";
		},
		blockId() {
			return this.settings.id;
		},
		blockStyle() {
			const styleSetting = this.settings.style || "";
			return [styleSetting]
				.join("\n")
				.replaceAll("#id", `#${this.blockId}`);
		},
		blockType() {
			return this.settings.type;
		},
		blockVariants() {
			const variants = [];
			if (this.settings.variants) {
				for (const [variantKey, variantValue] of Object.entries(
					this.settings.variants
				)) {
					// go through every variant saved that is not of the "_custom" key
					if (variantKey !== "_custom") {
						// variants defined without options are just toggles
						// and if they are true, then apply the variant key
						if (variantValue === true) {
							variants.push(variantKey);
						}
						// variants defined with specific string value options
						// should apply the selected value
						else {
							variants.push(variantValue);
						}
					}
				}
			}
			if (!variants.length) {
				variants.push("base");
			}
			return variants;
		},
		blockClasses() {
			const classList = ["block", `block_${this.blockType}`];
			this.blockVariants.forEach((variant) => {
				if (variant) {
					classList.push(`block_${this.blockType}_${variant}`);
				}
			});
			// anything added to the _custom key is a string w/ space delimited custom classes
			if (this.settings.variants?._custom) {
				classList.push(this.settings.variants._custom.split(" "));
			}
			return classList;
		},
	},
	methods: {
		insert(payload) {
			// relay it up to the forge control block
			this.$emit("insert", payload);
		},
		select(payload) {
			// relay it up to the forge control block
			this.$emit("select", payload);
		},
	},
};
</script>

<style lang="scss">
// styles universal to blocks (will refactor into CoreLibrary)

.block {
	min-height: 1rem;
	min-width: 1rem;
	z-index: 101;
	.block__body {
		z-index: 201;
	}
}

.block {
	position: relative;
	width: 100%;
	height: 100%;
	.block {
		background: transparent;
	}
	.block__body {
		position: relative;
		margin: 0 auto;
		padding: var(--_core__container_padding-top)
			var(--_core__container_padding-right)
			var(--_core__container_padding-bottom)
			var(--_core__container_padding-left);
		background: transparent;
		color: inherit;
		font-family: inherit;
		text-align: var(--_core__body_text-align);
		justify-content: var(--_core__body_justify-content);
		max-width: 100%;
		.block__container {
			max-width: var(--_core__container_max-width);
			width: 100%;
			margin-right: auto;
			margin-left: auto;
		}
	}

	--_core__list_margin-left: auto;
	--_core__list_margin-right: auto;
	&[class*="_align-left"] {
		.block__body {
			--_core__body_text-align: left;
			--_core__body_justify-content: flex-start;
			--_core__list_margin-left: 0;
			--_core__list_margin-right: auto;
		}
	}
	&[class*="_image-align-left"] {
		.block__body {
			.block__section_content {
				display: flex;
				justify-content: flex-start;
				img {
					margin: 0;
				}
			}
		}
	}
	&[class*="_image-align-center"] {
		.block__body {
			.block__section_content {
				display: flex;
				justify-content: center;
				img {
					margin: 0 auto;
				}
			}
		}
	}
	&[class*="_image-align-right"] {
		.block__body {
			.block__section_content {
				width: 100%;
				display: flex;
				justify-content: flex-end;
				img {
					margin: 0;
				}
			}
		}
	}

	&[class*="_align-center"] {
		.block__body {
			--_core__body_text-align: center;
			--_core__body_justify-content: center;
			--_core__list_margin-left: auto;
			--_core__list_margin-right: auto;
		}
	}

	&[class*="_align-right"] {
		.block__body {
			--_core__body_text-align: right;
			--_core__body_justify-content: flex-end;
			--_core__list_margin-left: auto;
			--_core__list_margin-right: 0;
		}
	}

	/*
	each block resets the default padding within
	otherwise nested blocks would inherit the padding of their parents
	*/
	--_core__container_padding-top: 0;
	--_core__container_padding-right: 0;
	--_core__container_padding-bottom: 0;
	--_core__container_padding-left: 0;

	/*

	padding related classes that can be applied to any block
	---
	every block has the variants 'padding-top', 'padding-bottom', and 'padding-horizontal'
	which will apply classes ending with things like 'pt-small' (padding-top small)
	so changing the value of the theme's variables inside each component
	to match whatever variant was selected will adjust the spacing on the fly

	*/
	&[class*="_pt-none"] {
		--_core__container_padding-top: 0;
	}
	&[class*="_pt-small"] {
		--_core__container_padding-top: 1rem;
	}
	&[class*="_pt-medium"] {
		--_core__container_padding-top: 2rem;
	}
	&[class*="_pt-large"] {
		--_core__container_padding-top: 3rem;
	}
	&[class*="_pt-huge"] {
		--_core__container_padding-top: 5rem;
	}
	&[class*="_pb-none"] {
		--_core__container_padding-bottom: 0;
	}
	&[class*="_pb-small"] {
		--_core__container_padding-bottom: 1rem;
	}
	&[class*="_pb-medium"] {
		--_core__container_padding-bottom: 2rem;
	}
	&[class*="_pb-large"] {
		--_core__container_padding-bottom: 3rem;
	}
	&[class*="_pb-huge"] {
		--_core__container_padding-bottom: 5rem;
	}
	&[class*="_px-none"] {
		--_core__container_padding-right: 0;
		--_core__container_padding-left: 0;
	}
	&[class*="_px-small"] {
		--_core__container_padding-right: 1rem;
		--_core__container_padding-left: 1rem;
	}
	&[class*="_px-medium"] {
		--_core__container_padding-right: 2rem;
		--_core__container_padding-left: 2rem;
	}
	&[class*="_px-large"] {
		--_core__container_padding-right: 3rem;
		--_core__container_padding-left: 3rem;
	}
	&[class*="_px-huge"] {
		--_core__container_padding-right: 5rem;
		--_core__container_padding-left: 5rem;
	}
	&[class*="_p-none"] {
		--_core__container_padding: 0;
	}
	&[class*="_p-small"] {
		--_core__container_padding: 1rem;
	}
	&[class*="_p-medium"] {
		--_core__container_padding: 2rem;
	}
	&[class*="_p-large"] {
		--_core__container_padding: 3rem;
	}
	&[class*="_p-huge"] {
		--_core__container_padding: 5rem;
	}
}
</style>
