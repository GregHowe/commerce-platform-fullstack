<template>
	<div class="block__body">
		<span class="icon--custom">
			<CoreIcon
				v-if="icon"
				:icon="icon"
				class="icon"
			/>
		</span>
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
		icon() {
			return this.settings?.icon;
		},
	},
};
</script>

<style lang="scss">
.block_icon {
	padding: 0.125rem; /* spacing */
	.block__body {
		.icon--custom {
			font-size: 1.4rem;
			& > svg {
				width: 1em;
				height: 1em;
			}
		}
	}
	&.block_icon_small {
		.block__body {
			.icon--custom {
				font-size: 1rem;
			}
		}
	}
	&.block_icon_medium {
		.block__body {
			.icon--custom {
				font-size: 2rem;
			}
		}
	}
	&.block_icon_large {
		.block__body {
			.icon--custom {
				font-size: 3rem;
			}
		}
	}
	&.block_icon_huge {
		.block__body {
			.icon--custom {
				font-size: 5rem;
			}
		}
	}
}
</style>
