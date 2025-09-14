<template>
	<div
		:class="{
			forge__control: true,
			'forge__control--horizontal': isHorizontal,
		}"
	>
		<div class="forge__control_insert forge__control_insert--before">
			<template v-if="nestedLevel === 0">
				<v-btn
					color="accent text-body-2 font-weight-bold"
					class="white--text"
					small
					@click="chooseBefore"
				>
					+ Section
				</v-btn>
			</template>
			<v-btn
				v-else
				icon
				class="white-behind"
				@click="insertBefore"
			>
				<v-icon>mdi-plus-circle</v-icon>
			</v-btn>
		</div>
		<div
			v-if="blockTypeRequiresChild && !hasChildren"
			class="forge__control_insert forge__control_insert--inside"
		>
			<ForgeButton
				text
				@click="insertInside"
			>
				Add Block
			</ForgeButton>
		</div>
		<div class="forge__control_insert forge__control_insert--after">
			<template v-if="nestedLevel === 0">
				<v-btn
					color="accent"
					class="white--text text-body-2 font-weight-bold"
					small
					@click="chooseAfter"
				>
					+ Section
				</v-btn>
			</template>
			<v-btn
				v-else
				color="blue"
				icon
				class="white-behind"
				@click="insertAfter"
			>
				<v-icon>mdi-plus-circle</v-icon>
			</v-btn>
		</div>
	</div>
</template>
<script>
import schema from "~/schemas/block.json";
export default {
	props: {
		hasChildren: {
			type: Boolean,
			default: false,
		},
		blockType: {
			type: String,
			default: null,
		},
		isHorizontal: {
			type: Boolean,
			default: false,
		},
		nestedLevel: {
			type: Number,
			default: 0,
		},
	},
	computed: {
		blockTypeSchema() {
			if (!this.blockType) {
				return null;
			}
			return schema.types[this.blockType];
		},
		blockTypeRequiresChild() {
			return this.blockTypeSchema?.requireChild || false;
		},
	},
	methods: {
		chooseAfter() {
			this.$emit("choose", {
				placement: 1,
			});
		},
		chooseBefore() {
			this.$emit("choose", {
				placement: -1,
			});
		},
		insertAfter() {
			this.$emit("insert", {
				placement: 1,
				settings: {},
			});
		},
		insertBefore() {
			this.$emit("insert", {
				placement: -1,
				settings: {},
			});
		},
		insertInside() {
			this.$emit("insert", {
				placement: 0,
				settings: {},
			});
		},
	},
};
</script>

<style lang="scss">
.forge__control {
	.forge__control_insert {
		cursor: pointer;
		position: absolute;
		z-index: 1130;
		justify-content: center;
		opacity: 1;
		text-align: center;
		text-decoration: none;
		margin: 0 auto;
		right: 0;
		left: 0;
		.v-btn {
			pointer-events: all;
		}
		&.active {
			display: flex;
		}
		&:hover,
		&:focus {
			opacity: 0.8;
		}
		&.forge__control_insert--before {
			top: -5px;
			transform: translateY(-50%);
		}
		&.forge__control_insert--after {
			top: auto;
			bottom: -5px;
			transform: translateY(50%);
		}
		&.forge__control_insert--inside {
			top: 0.75rem;
			bottom: 0.75rem;
			margin: auto;
		}
	}
	&.forge__control--horizontal {
		.forge__control_insert {
			margin: auto 0;
			top: 50%;
			bottom: auto;
			&.forge__control_insert--before {
				right: auto;
				left: 0;
				transform: translate(-50%, -50%);
			}
			&.forge__control_insert--after {
				left: auto;
				right: 0;
				transform: translate(50%, -50%);
			}
		}
	}
	.white-behind:after {
		content: "";
		display: block;
		position: absolute;
		top: 10px;
		right: 10px;
		bottom: 10px;
		left: 10px;
		background: white;
		border-radius: 50%;
		z-index: -1;
	}
}
</style>
