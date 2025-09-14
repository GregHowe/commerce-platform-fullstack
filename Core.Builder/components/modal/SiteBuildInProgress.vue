<script>
import { mapState, mapGetters } from "vuex";

export default {
	data() {
		return {
			showDialog: false,
			disable: false,
		};
	},
	computed: {
		...mapState({
			workingSite: (state) => state.site.workingSite,
			justPublished: (state) => state.buildQueue.justPublished,
		}),
		...mapGetters({
			firstName: "user/firstName",
			isQueued: "buildQueue/isQueued",
		}),
		siteId() {
			return this.workingSite?.id || "";
		},
		publishInProgress: {
			get() {
				return this.justPublished || this.isQueued;
			},
			set() {},
		},
	},
};
</script>
<template>
	<v-dialog
		v-model="disable"
		persistent
		transition="fab-transition"
		max-width="550"
		open-delay="500"
	>
		<v-card>
			<v-toolbar
				color="primary"
				dark
				><v-icon
					size="24"
					class="mr-2"
					>mdi-lock</v-icon
				>Site build in progress</v-toolbar
			>
			<v-card-text class="pa-12">
				<div class="text-h3 pb-4">Almost there!</div>
				<div class="black--text">
					Your site is being published or reviewed, and is frozen from
					changes. We'll let you know when there's an update.
				</div>
				<BuildQueueSmall class="mt-6 mx-auto" />
			</v-card-text>
			<v-card-actions class="justify-space-around">
				<v-btn
					text
					@click="$router.go(-1)"
					>Close</v-btn
				>
			</v-card-actions>
		</v-card>
	</v-dialog>
</template>
