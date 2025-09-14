<template>
	<div class="block__body">
		<hr class="divider" />
		<div
			v-if="blockContent"
			class="block__section_content"
		>
			{{ blockContent }}
		</div>
	</div>
</template>

<script>
export default {
	name: "CoreBlockDivider",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		blockContent() {
			return this.settings.content || null;
		},
	},
};
</script>

<style lang="scss">
// block level styles that need to match the generated frontend
// should maybe be stored outside of the component
.block_divider {
	.block__body {
		padding: 1rem 0;
		.divider {
			display: block;
			margin: 0;
			padding: 0;
			height: 0;
			background: none;
			border-style: solid;
			border-color: var(--_core__divider_color);
			border-width: 0 0 2px;
		}
	}

	&.block_divider_spacer {
		height: 2rem;
		.block__body .divider {
			border: 0;
		}
		&.block_divider_small {
			height: 1rem;
		}
		&.block_divider_large {
			height: 3rem;
		}
		&.block_divider_huge {
			height: 5rem;
		}
	}

	&.block_divider_dashed {
		.block__body .divider {
			border-style: dashed;
		}
	}

	&.block_divider_double {
		.block__body .divider {
			border-style: double;
			border-width: 0 0 4px;
		}
	}

	&.block_divider_dotted {
		.block__body .divider {
			border-style: dotted;
		}
	}

	&.block_divider_small {
		.block__body .divider {
			border-width: 0 0 1px;
		}
	}

	&.block_divider_large {
		.block__body .divider {
			border-width: 0 0 4px;
		}
	}

	&.block_divider_huge {
		.block__body .divider {
			border-width: 0 0 8px;
		}
	}

	&.block_divider_primary {
		.block__body .divider {
			border-color: var(--core__color_primary);
		}
	}

	&.block_divider_secondary {
		.block__body .divider {
			border-color: var(--core__color_secondary);
		}
	}

	&.block_divider_dark {
		.block__body .divider {
			border-color: var(--core__color_dark);
		}
	}

	&.block_divider_dark-alt {
		.block__body .divider {
			border-color: var(--core__color_dark-alt);
		}
	}

	&.block_divider_light {
		.block__body .divider {
			border-color: var(--core__color_light);
		}
	}

	&.block_divider_light-alt {
		.block__body .divider {
			border-color: var(--core__color_light-alt);
		}
	}

	&.block_divider_contrast {
		.block__body .divider {
			border-color: var(--core__color_contrast);
		}
	}
} // special cases for dividers within column or row containers flips them vertical
.block_container_grid,
.block_container_columns {
	.block__section_main > .block_divider {
		/*specificity overrides main container style*/
		flex: 0;
		width: auto;
		.block__body {
			height: 100%;
			padding: 0 1rem;
			.divider {
				width: 0;
				height: 100%;
				border-width: 0 2px 0 0;
				margin: 0 auto;
			}
			.block__section_content {
				height: 100%;
			}
		}

		&.block_divider_double {
			.block__body .divider {
				border-width: 0 4px 0 0;
			}
		}

		&.block_divider_small {
			.block__body .divider {
				border-width: 0 1px 0 0;
			}
		}

		&.block_divider_large {
			.block__body .divider {
				border-width: 0 4px 0 0;
			}
		}
	}
}
</style>
