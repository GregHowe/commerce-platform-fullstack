<template>
	<CoreBlock
		v-if="showBanner"
		:settings="pencilBannerSettings"
	/>
</template>

<script>
export default {
	name: "CoreBlockFooterBanner",
	props: {
		site: {
			type: Object,
			required: true,
		},
	},
	computed: {
		banner() {
			return this.site.footer?.banner || {};
		},
		showBanner() {
			return !!this.site.footer?.showBanner || false;
		},
		bodyCopy() {
			return this.banner.bodyCopy;
		},
		buttonText() {
			return this.banner.buttonText;
		},
		buttonLink() {
			return this.banner.buttonLink;
		},
		pencilBannerSettings() {
			return {
				id: "f36wnm",
				type: "pencil-banner",
				variants: {
					alignment: "center",
					hidden: false,
					"text-align": "align-center",
				},
				bodyCopy: this.bodyCopy,
				buttonText: this.buttonText,
				buttonLink: this.buttonLink,
				newTab: this.banner?.newTab || null,
				closeButton: this.banner?.closeButton || null,
			};
		},
	},
};
</script>
