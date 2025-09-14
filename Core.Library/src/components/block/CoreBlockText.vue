<template>
	<component
		:is="blockTag"
		v-if="blockContent"
		class="block__body"
	>
		{{ blockContent }}
	</component>
</template>

<script>
export default {
	name: "CoreBlockText",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		blockContent() {
			return this.settings?.text || null;
		},
		blockTag() {
			return this.settings?.tag || "div";
		},
	},
};
</script>

<style lang="scss">
.block_text {
	.block__body {
		font-size: var(--_core__container_font-size);
		text-transform: none;
		font-weight: 400;
		font-style: normal;
		color: var(--_core__container_color);
		font-family: var(--_core__container_font-family);
		text-align: var(_core__container_text-align);
		margin: 0.4rem 0;
	}

	&.block_text_overline {
		.block__body {
			color: var(--_core__overline_color);
			font-size: var(--_core__overline_font-size);
			text-transform: var(--_core__overline_text-transform);
			font-family: var(--_core__overline_font-family);
		}
	}

	&.block_text_title,
	&.block_text_headline {
		.block__body {
			line-height: var(--_core__heading_line-height);
			// color: var(--_core__heading_color);
			text-transform: var(--_core__heading_text-transform);
			font-family: var(--_core__heading_font-family);
			font-weight: 500;			
		}
		h1.block__body {
			font-size: var(--_core__heading_font-size--h1);
			font-family: 	var(--_core__heading_font-family--h1);
		}
		h2.block__body {
			font-size: var(--_core__heading_font-size--h2);
			font-family: 	var(--_core__heading_font-family--h2);
			color: var(	--_core__heading_color--h2);
		}
		h3.block__body {
			font-size: var(--_core__heading_font-size--h3);
			font-family: 	var(--_core__heading_font-family--h3);
		}
		h4.block__body {
			font-size: var(--_core__heading_font-size--h4);
			font-family: 	var(--_core__heading_font-family--h4);
		}
		h5.block__body {
			font-size: 22px;
			font-family: 	var(--_core__heading_font-family--h5);
			text-transform: var(--_core__heading_text-transform--h5);
			font-weight: 600;
		}
	}

	&.block_text_subtitle,
	&.block_text_subline {
		.block__body {
			font-size: var(--_core__subline_font-size);
			line-height: var(--_core__subline_line-height);
			color: var(--_core__subline_color);
			text-transform: var(--_core__subline_text-transform);
			font-family: var(--_core__subline_font-family);
			margin: 0.4rem 0 0.8rem;
		}
	}

	&.block_text_footnote,
	&.block_text_disclosure {
		.block__body {
			font-size: var(--_core__disclosure_font-size);
			color: var(--_core__disclosure_color);
			font-style: var(--_core__disclosure_font-style);
			line-height: var(--_core__disclosure_line-height);
		}
	}

	&.block_text_bold {
		.block__body {
			font-weight: 600;
		}
	}

	&.block_text_italic {
		.block__body {
			font-style: italic;
		}
	}

	&.block_text_headline + .block_rich-text,
	&.block_text_headline + .block_text {
		// margin-top: 0.6em;
		margin-bottom: 0.6em;
	}
}
</style>
