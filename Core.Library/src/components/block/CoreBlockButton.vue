<template>
	<div class="block__body">
		<a
			v-if="!isInternal"
			role="button"
			:href="buttonUrl"
			:target="newTabTarget"
			class="button"
			@click="clickEvent"
		>
			<span class="button_icon--custom">
				<CoreIcon
					v-if="settings.icon"
					:icon="settings.icon"
					name="buttonIcon"
					class="button_icon"
				/>
			</span>
			<span class="button_label button_icon--down">
				<span class="button_label_text">{{ buttonLabel }}</span>
				<CoreIcon
					icon="down-arrow"
					class="button_icon button_i"
				/>
			</span>
		</a>
		<NuxtLink
			v-else
			role="button"
			:to="buttonUrl"
			:target="newTabTarget"
			class="button"
			@click="clickEvent"
		>
			<span class="button_icon--custom">
				<CoreIcon
					v-if="settings.icon"
					:icon="settings.icon"
					name="buttonIcon"
					class="button_icon"
				/>
			</span>
			<span class="button_label button_icon--down">
				<span class="button_label_text">{{ buttonLabel }}</span>
				<CoreIcon
					icon="down-arrow"
					class="button_icon button_i"
				/>
			</span>
		</NuxtLink>
	</div>
</template>

<script>
export default {
	name: "CoreBlockButton",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		newTabTarget() {
			return this.settings.newTab ? "_blank" : null;
		},
		buttonUrl() {
			return this.settings?.url || null;
		},
		buttonLabel() {
			return this.settings?.label || "Button Label";
		},
		clickEvent() {
			return this.settings?.event || this.noEventFound;
		},
		isInternal() {
			return this.settings?.url?.charAt(0) == "/";
		},
	},
	methods: {
		noEventFound() {},
	},
};
</script>

<style lang="scss">
.block_button {
	// padding: 0.125rem; /* spacing */
	font-family: var(--_core__button_font-family);
	.block__body {
		width: fit-content;
		white-space: nowrap;
	}

	&,
	&.block_button_primary {
		.button {
			cursor: pointer;
			padding: 0.75rem 1.25rem;
			display: flex;
			flex-direction: row;
			background-color: var(--core__color_primary);
			border: 1px solid var(--core__color_primary);
			border-radius: var(--_core__button-primary_border-radius);
			color: var(--core__color_white);
			text-decoration: none;
		}
		.button_label {
			display: flex;
			align-items: center;
		}
		&.block_button_outline {
			.button {
				background-color: transparent;
				color: var(--core__color_primary);
			}
		}
		&.block_button_invert {
			.button {
				background-color: var(--core__color_white);
				border-color: var(--core__color_white);
				color: var(--core__color_primary);
			}
			&.block_button_outline {
				.button {
					background-color: transparent;
					border-color: var(--core__color_white);
					color: var(--core__color_white);
				}
			}
		}
	}

	&.block_button_secondary {
		.button {
			background-color: var(--core__color_white);
			border-color: var(--core__color_primary);
			color: var(--core__color_primary);
		}
		&.block_button_outline {
			.button {
				background-color: transparent;
				border-color: var(--core__color_primary);
				color: var(--core__color_primary);
			}
		}
		&.block_button_invert {
			.button {
				background-color: var(--core__color_primary);
				border-color: var(--core__color_primary);
				color: var(--core__color_primary);
			}
			&.block_button_outline {
				.button {
					background-color: #ff0000;
					border-color: var(--core__color_primary);
					color: var(--core__color_primary);
				}
			}
		}
	}

	&.block_button_tertiary {
		.button {
			background: transparent;
			border-color: transparent;
			color: var(--core__color_primary);
			padding-left: 0;
			font-size: 18px;

			.button_icon--custom svg {
				display: flex;
			}
		}
	}

	&.block_button_link {
		.button {
			background: transparent;
			border-color: transparent;
			color: var(--_core__link_color);
			text-decoration: var(--_core__link_text-decoration);
			padding: 0;
		}
	}

	&.block_button_text {
		.button {
			background: transparent;
			border-color: transparent;
			color: var(--core__color_primary);
			padding-left: 0;
			font-weight: 700;
			font-size: 18px;

			.button_icon--custom svg {
				display: flex;
			}
		}
	}

	&.block_button_align-left {
		.block__body {
			margin-left: 0;
		}
	}

	&.block_button_align-right {
		.block__body {
			margin: 0 auto;
		}
	}

	&.block_button_align-right {
		.block__body {
			margin-right: 0;
		}
	}

	.button_icon--custom svg {
		margin: 0 1rem 0 -0.5rem;
	}

	.button_icon--down svg {
		display: none;
	}

	&.block_button_scroll {
		.button {
			background: transparent;
			border-color: transparent;
			color: var(--_core__button_primary-background);
			align-items: center;
		}
		.button_label {
			flex-direction: column;
		}
		.button_icon--custom svg {
			display: inline;
		}
		.button_icon--down svg {
			display: block;
			margin: -0.25rem auto -1.25rem;
		}
	}

	&.block_button_icon-right {
		.button {
			flex-direction: row-reverse;
			.button_icon--custom svg {
				margin: 0 -0.5rem 0 1rem;
			}
		}
	}
}
</style>
