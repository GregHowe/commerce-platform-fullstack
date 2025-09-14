<template>
	<v-toolbar
		v-show="visible"
		dense
		floating
		style="position: fixed; bottom: 0; z-index: 4; height: 30px"
		class="toolbar-footer"
		short
		rounded
		outlined
		color="rgba(255, 255, 255, .9)"
		elevation="0"
	>
		<GlobalAuthWrapper :restricted-to="['Agent']">
			<ForgeButton
				x-small
				outlined
				class="text-capitalize mx-1 toolbar-btn"
				:restricted-to="['Agent']"
				@click="publish"
			>
				<v-icon
					color="orange"
					class="mr-2"
					x-small
				>
					mdi-shield-star-outline
				</v-icon>
				Publish
			</ForgeButton>
		</GlobalAuthWrapper>

		<GlobalAuthWrapper :restricted-to="['Agent']">
			<ForgeButton
				x-small
				class=""
				color="primary"
				:disabled="disableButtons"
				@click="save"
			>
				Save
			</ForgeButton>
		</GlobalAuthWrapper>

		<GlobalAuthWrapper
			class="ma-0 pa-0"
			:restricted-to="['Agent']"
		>
			<ForgeButton
				outlined
				x-small
				class="mx-1"
				color="primary"
				:disabled="disableButtons"
				:class="{ activeUndo: hasWorkingChanges }"
				@click="reset"
			>
				Undo
				<v-icon small>mdi-undo-variant</v-icon>
			</ForgeButton>
		</GlobalAuthWrapper>

		<div>
			<v-snackbar
				v-model="alert.type"
				app
				height="10"
				:type="alert.type"
				color="green"
			>
				{{ alert.content }}
			</v-snackbar>
		</div>
	</v-toolbar>
</template>
<script>
import { mapActions, mapState, mapGetters } from "vuex";
export default {
	data() {
		return {
			alert: {
				content: "",
				type: "",
			},
			autosaveEnabled: true,
			autosaveInterval: {},
			loaderPercent: 0,
			visible: false,
		};
	},

	computed: {
		...mapGetters({
			isLoadingAnything: "interface/isLoadingAnything",
			validationFailedAny: "validation/validationFailedAny",
			validationFailedMessages: "validation/validationFailedMessages",
			hasWorkingChanges: "site/hasWorkingChanges",
		}),
		...mapState({
			workingSite: (state) => state.site.workingSite,
		}),

		disableButtons() {
			return this.isLoadingAnything || !this.hasWorkingChanges;
		},
		loaderPercentInuse() {
			return Math.floor(this.loaderPercent / 10) * 10 + 10;
		},
	},
	beforeMount() {
		window.addEventListener("scroll", this.handleScroll);
	},
	beforeDestroy() {
		window.removeEventListener("scroll", this.handleScroll);
	},

	methods: {
		...mapActions("site", [
			"saveWorkingSite",
			"resetWorkingSite",
			"saveAndPublishWorkingSite",
		]),
		handleScroll() {
			this.visible = window.scrollY != 0;
		},
		reset() {
			this.resetWorkingSite();
			this.$store.commit("interface/setActiveSidebarKey", null);
		},
		validate() {
			if (this.validationFailedAny) {
				this.alert = {
					content: this.validationFailedMessages.toString(),
					type: "error",
				};
				return false;
			}
			return true;
		},
		async publish() {
			if (this.validate()) {
				const response = await this.saveAndPublishWorkingSite();
				if (response) {
					this.alert = {
						content: "Submitted",
						type: "success",
					};
				} else {
					this.alert = {
						content: "Error!",
						type: "error",
					};
				}
			}
			setTimeout(() => {
				this.alert = { content: "", type: "" };
			}, 5000);
		},
		async save() {
			if (this.validate()) {
				const response = await this.saveWorkingSite();
				if (response) {
					this.alert = {
						content: "Saved",
						type: "success",
					};
				} else {
					this.alert = {
						content: "Error!",
						type: "error",
					};
				}
			}
			setTimeout(() => {
				this.alert = { content: "", type: "" };
			}, 5000);
		},
	},
};
</script>
<style
	scoped
	lang="scss"
>
.toolbar-footer {
	left: 0;
	right: 0;
	width: 100%;
	@include breakpoint_up(xl) {
		left: 256px;
		right: 256px;
	}
	::v-deep .v-toolbar {
		&__content {
			height: 28px !important;
		}
	}
}
</style>
