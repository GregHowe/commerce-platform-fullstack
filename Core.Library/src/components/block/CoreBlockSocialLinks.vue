<template>
	<div class="block__body">
		<div class="block__header_group">
			<div
				v-if="blockHeadline"
				class="block__headline"
			>
				{{ blockHeadline }}
			</div>
		</div>
		<div class="block__icon_group">
			<div
				v-if="blockFacebook"
				class="block__facebook"
			>
				<a
					class="block__link"
					:href="blockFacebook"
				>
					<CoreIconFacebook />
				</a>
			</div>
			<div
				v-if="blockLinkedin"
				class="block__linkedin"
			>
				<a
					:href="blockLinkedin"
					class="block__link"
				>
					<CoreIconLinkedin />
				</a>
			</div>
			<div
				v-if="blockInstagram"
				class="block__instagram"
			>
				<a
					:href="blockInstagram"
					class="block__link"
				>
					<CoreIconInstagram />
				</a>
			</div>
			<div
				v-if="blockTwitter"
				class="block__twitter"
			>
				<a
					:href="blockTwitter"
					class="block__link"
				>
					<CoreIconTwitter />
				</a>
			</div>
			<div
				v-if="blockYoutube"
				class="block__youtube"
			>
				<a
					:href="blockYoutube"
					class="block__link"
				>
					<CoreIconYoutube />
				</a>
			</div>
		</div>
		<div class="block__header_group">
			<div
				v-if="blockUsername"
				class="block__username"
			>
				{{ blockUsername }}
			</div>
		</div>
	</div>
</template>

<script>
import { renderData } from "@libraryHelpers/dataComponents.js";

export default {
	name: "CoreBlockSocialLinks",
	props: {
		settings: {
			type: Object,
			required: true,
		},
		dataSite: {
			type: Object,
			default: () => {},
		},
	},
	computed: {
		blockHeadline() {
			return this.settings.headline || null;
		},
		blockUsername() {
			return this.settings.username || null;
		},
		blockFacebook() {
			return renderData(this.settings.facebook, this.dataSite);
		},
		blockLinkedin() {
			return renderData(this.settings.linkedin, this.dataSite);
		},
		blockInstagram() {
			return this.settings.instagram || "";
		},
		blockTwitter() {
			return renderData(this.settings.twitter, this.dataSite);
		},
		blockYoutube() {
			return this.settings.youtube || "";
		},
	},
};
</script>

<style lang="scss">
.block_social-links {
	.block__body {
		display: flex;
		flex-direction: column;
		justify-content: var(--_core__body_justify-content);
		padding: 1rem;
		background: transparent;
		color: var(--_core__container_color);

		.block__link {
			display: flex;
			flex-direction: column;
			justify-content: var(--_core__body_justify-content);
			margin: 0 0.5rem;
			height: 100%;
			color: var(--_core__container_color);
		}

		.block__icon_group {
			display: flex;
			flex-direction: row;
			justify-content: var(--_core__body_justify-content);
			text-align: var(--_core__body_text-align);
			padding: 0.8rem 0;
		}

		.block__header_group {
			display: flex;
			flex-direction: column;
			justify-content: var(--_core__body_justify-content);
			text-align: var(--_core__body_text-align);
		}
	}

	&.block_social-links_contrast .block__body {
		background: var(--_core__container_color);
		color: var(--core__color_light-alt);
		a {
			color: var(--core__color_light-alt);
		}
	}

	&.block_social-links_background .block__body {
		background: var(--_core__container_background);
		color: var(--_core__container_color);
	}
	&.block_social-links_slim {
		.block__body {
			background: transparent;
			padding: 0;
			.block__link {
				margin: 0 0.4rem;
				padding: 0;
				height: auto;
			}
			.block__icon_group {
				padding: 0;
			}
		}
		&.block_social-links_align-left .block__body {
			.block__link:first-child {
				margin-left: 0;
			}
		}
		&.block_social-links_align-right .block__body {
			.block__link:last-child {
				margin-right: 0;
			}
		}
	}
	&.block_social-links_align-left .block__body {
		align-items: flex-start;
	}
	&.block_social-links_align-right .block__body {
		align-items: flex-end;
	}
}
</style>
