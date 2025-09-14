<template>
	<div class="">
		<div
			transition="fade-transition"
			elevation="0"
			class="d-flex site-saver"
		>
			<GlobalAuthWrapper :restricted-to="['Agent']">
				<ForgeButton
					outlined
					class="text-body-2 text-capitalize mx-1"
					:restricted-to="['Agent']"
					@click="publish"
				>
					<v-icon
						color="orange"
						class="mr-2"
					>
						mdi-shield-star-outline
					</v-icon>
					Publish
				</ForgeButton>
			</GlobalAuthWrapper>

			<div>
				<v-checkbox
					v-model="autosaveEnabled"
					small
					outlined
					color="orange"
					class="d-inline-block"
					style="position: relative; top: 4px; left: 10px"
					dense
				/>
				<span class="text-caption">Autosave</span>
			</div>

			<div class="d-inline mx-1">
				<v-progress-circular
					v-if="autosaveEnabled && hasWorkingChanges"
					style="top: 8px"
					:width="3"
					size="18"
					:value="loaderPercentInuse"
					color="orange"
				/>
			</div>
			<GlobalAuthWrapper :restricted-to="['Agent']">
				<ForgeButton
					outlined
					class="mx-1"
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
		</div>
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
	</div>
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
			autosaveEnabled: this?.$config?.AUTOSAVE_DEFAULT || false,
			autosaveInterval: {},
			loaderPercent: 0,
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
	mounted() {
		try {
			this.$gsap.to(this, 15, {
				loaderPercent: 100,
				ease: "none",
				repeat: -1,
				onRepeat: async () => {
					if (
						this.autosaveEnabled &&
						this.validate() &&
						this.hasWorkingChanges
					) {
						await this.saveWorkingSite();
					}
				},
			});
		} catch (err) {
			console.log(err);
		}
	},
	methods: {
		...mapActions("site", [
			"saveWorkingSite",
			"resetWorkingSite",
			"saveAndPublishWorkingSite",
		]),
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
></style>
