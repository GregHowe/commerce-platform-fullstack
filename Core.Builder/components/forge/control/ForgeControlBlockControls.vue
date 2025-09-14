<template>
	<v-container
		ref="controlbox"
		class="forge__control_block_controller"
	>
		<v-row class="font-weight-medium text-capitalize controls-border">
			<v-col
				cols="12"
				class="pa-0 pt-0 mt-0 mb-0 mb-1"
			>
				<div class="text-body-2 font-weight-medium ma-0 pa-0">
					Block Controls - <strong>{{ blockType }}</strong>
				</div>

				<v-btn
					v-if="!isHorizontal"
					icon
					x-small
					dark
					color="white"
					:disabled="isFirstChild"
					@click="$emit('moveup')"
				>
					<v-icon>mdi-arrow-up</v-icon>
				</v-btn>
				<v-btn
					v-if="!isHorizontal"
					icon
					x-small
					dark
					color="white"
					:disabled="isLastChild"
					@click="$emit('movedown')"
				>
					<v-icon>mdi-arrow-down</v-icon>
				</v-btn>
				<v-btn
					v-if="isHorizontal"
					icon
					dark
					x-small
					color="white"
					:disabled="isFirstChild"
					@click="$emit('moveleft')"
				>
					<v-icon>mdi-arrow-left</v-icon>
				</v-btn>
				<v-btn
					v-if="isHorizontal"
					icon
					dark
					x-small
					color="white"
					:disabled="isLastChild"
					@click="$emit('movedown')"
				>
					<v-icon>mdi-arrow-right</v-icon>
				</v-btn>
				<v-btn
					icon
					dark
					x-small
					color="white"
					@click="$emit('copy-block')"
				>
					<v-icon>mdi-content-copy</v-icon>
				</v-btn>
				<v-btn
					icon
					dark
					x-small
					color="white"
					@click="$emit('cut-block')"
				>
					<v-icon>mdi-content-cut</v-icon>
				</v-btn>
				<v-btn
					icon
					dark
					x-small
					color="white"
					:disabled="!copiedBlock"
					@click="insertInside"
				>
					<v-icon>mdi-content-paste</v-icon>
				</v-btn>
				<v-btn
					depressed
					x-small
					class="mb-0 pb-0 mr-0 pr-0 accent"
					@click="$emit('remove')"
				>
					<v-icon>mdi-delete-outline</v-icon>
				</v-btn>
			</v-col>
		</v-row>
	</v-container>
</template>

<script>
import { mapState } from "vuex";

export default {
	props: {
		blockId: {
			type: String,
			default: "",
		},
		blockType: {
			type: String,
			default: null,
		},
		isHorizontal: {
			type: Boolean,
			default: false,
		},
		isFirstChild: {
			type: Boolean,
			default: false,
		},
		isLastChild: {
			type: Boolean,
			default: false,
		},
		copiedBlock: {
			type: Boolean,
			default: false,
		},
	},
	computed: {
		isOnlyChild() {
			return this.isFirstChild && this.isLastChild;
		},
		...mapState({
			workingBlock: (state) => state.site.workingBlock,
		}),
	},
	watch: {
		workingBlock() {
			this.$refs.controlbox.scrollIntoView({
				behavior: "smooth",
				block: "end",
				inline: "nearest",
			});
		},
	},
	methods: {
		insertInside() {
			this.$emit("paste-block", {
				placement: 0,
				settings: {},
			});
		},
	},
};
</script>

<style lang="scss">
.v-application .page .forge__control_block_controller {
	position: absolute;
	top: 20px;
	right: 40px;
	height: auto;
	width: auto;
	pointer-events: none;
	z-index: 9999999;

	.v-btn {
		pointer-events: all;
	}
	.v-btn[disabled] {
		pointer-events: none;
	}
}
.controls-border {
	top: 0;
	border: solid 1px #2b9ce0;
	border-radius: 3px;
	background: #2b9ce0;
	color: white;
	-webkit-box-shadow: 5px 3px 5px 0px rgba(0, 0, 0, 0.45);
	-moz-box-shadow: 5px 3px 5px 0px rgba(0, 0, 0, 0.45);
	box-shadow: 5px 3px 5px 0px rgba(0, 0, 0, 0.45);
}
</style>
