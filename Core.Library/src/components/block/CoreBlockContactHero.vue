<template>
	<div
		:style="{
			...background,
		}"
	>
		<div class="contact-card-inner">
			<img
				:src="mobileImage"
				class="contact-card-bg-img"
				style="height: 100%; width: 100%"
			/>
			<div class="contact-hero block_contact-card">
				<CoreBlockContactCard
					:settings="settingsObj"
					:data-site="dataSiteObj"
				/>
			</div>
		</div>
	</div>
</template>

<script>
export default {
	name: "CoreBlockContactHero",
	props: {
		settings: {
			type: Object,
			required: true,
		},
		dataSite: {
			type: Object,
			required: true,
		},
		isBuilderMobile: {
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
					"background-size": "cover",
				};
			}
			return null;
		},
		mobileImage() {
			return this.settings.backgroundMediaSrc;
		},
		settingsObj() {
			return this.settings;
		},
		dataSiteObj() {
			return this.dataSite;
		},
		isMobile() {
			if (this.isBuilderMobile) {
				return true;
			}
			// breakpoint needs to correspond with Core.Builder/assets/scss/mixins.scss
			const breakpoint = 1024;
			return this.windowWidth && this.windowWidth < breakpoint;
		},
	},
};
</script>

<style lang="scss">
.block_contact-hero {
	> div {
		padding: 0;
		margin: 0;
		width: 100%;
		padding: var(--_core__container_padding-top)
			var(--_core__container_padding-right)
			var(--_core__container_padding-bottom)
			var(--_core__container_padding-left);
		@include breakpoint_down(lg) {
			background-image: none !important;
		}
	}
	.contact-card-inner {
		max-width: var(--_core__container_max-width);
		margin: 0 auto;
	}
	.contact-card-bg-img {
		display: none;
		@include breakpoint_down(lg) {
			display: block;
			max-width: 100%;
			padding: 0;
		}
	}

	.contact-hero.block_contact-card {
		margin: 0;
		max-width: fit-content;
		min-width: fit-content;
		width: fit-content;
		padding: 44px 0 59px;
		@include breakpoint_down(lg) {
			width: 100%;
			max-width: 100%;
			min-width: auto;
			padding: 0;
		}
		.block__body {
			background-color: transparent;
			color: var(--_core__contactcard_font-color);
			font-family: var(--_core__contactcard_font-family);
		}
		> .block__body {
			background-color: var(
				--_core__contactcardhero_container_background
			);
			padding: 0;
			margin: 0;
			width: 100%;

			.block__section_info {
				padding: 40px 60px;
				@include breakpoint_down(lg) {
					padding: 30px 40px;
				}
			}
		}
	}
}
</style>
