<template>
	<v-app-bar
		width="100%"
		height="60px"
		class="views pt-1"
	>
		<v-row justify-space-around>
			<v-col cols="12">
				<div class="d-flex align-start">
					<v-btn
						v-if="workingSiteStaging"
						outlined
						:href="workingSiteStaging"
						target="_blank"
						><v-icon class="mr-2">mdi-eye</v-icon>Preview
						Staging</v-btn
					>
					<v-spacer />
					<SiteSaver />
					<v-spacer />
					<div class="toggle_buttons">
						<ForgeButton
							x-small
							outlined
							class="mx-1 pa-2 py-4"
							:class="{ active: activeView === 'desktop' }"
							@click="$emit('view', 'desktop')"
						>
							<v-icon small>mdi-monitor</v-icon>
						</ForgeButton>
						<ForgeButton
							x-small
							outlined
							class="mx-1 pa-2 py-4"
							:class="{ active: activeView === 'mobile' }"
							@click="$emit('view', 'mobile')"
						>
							<v-icon small>mdi-cellphone</v-icon>
						</ForgeButton>
						<ForgeButton
							x-small
							outlined
							class="mx-1 pa-2 py-4"
							:class="{ active: activeView === 'fullscreen' }"
							@click="$emit('view', 'fullscreen')"
						>
							<v-icon small>mdi-fullscreen</v-icon>
						</ForgeButton>
					</div>
				</div>
			</v-col>
		</v-row>
		<v-alert
			v-if="complianceError"
			type="error"
			dismissable
		>
			{{ complianceError }}
		</v-alert>
	</v-app-bar>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
export default {
	props: {
		activeView: {
			// passed in from the parent
			type: String,
			required: true, // this check fails if the active view ever gets unset
		},
	},
	data() {
		return {
			complianceError: null,
		};
	},
	computed: {
		...mapGetters({
			workingSiteAzureURL: "site/workingSiteAzureURL",
			validationFailedAny: "validation/validationFailedAny",
			validationFailedMessages: "validation/validationFailedMessages",
			hasWorkingChanges: "site/hasWorkingChanges",
		}),
		workingSiteStaging() {
			return this?.workingSiteAzureURL?.staging || "";
		},
	},

	beforeMount() {
		window.addEventListener("scroll", this.handleScroll);
	},
	beforeDestroy() {
		window.removeEventListener("scroll", this.handleScroll);
	},
	methods: {
		handleScroll() {
			if (window.scrollY != 0) {
				this.barheight = "30px";
				this.dense = true;
			} else {
				this.barheight = "60px";
				this.dense = false;
			}
		},
	},
};
</script>

<style lang="scss">
.theme--light.v-btn.active {
	background: black;
	color: white;
	pointer-events: none;
}
</style>
