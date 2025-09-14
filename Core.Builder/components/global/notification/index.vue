<template>
	<v-snackbar
		v-model="notificationModel.description"
		:timeout="15000"
		left
		:style="`cursor: ${notification.link ? 'pointer' : 'default'}`"
	>
		<div
			class="mt-0 pt-0 text-body text-uppercase font-weight-bold"
			@click="notificationClicked"
		>
			<v-icon
				:color="notificationModel.color"
				class="mr-2"
				>{{ notificationModel.icon }}</v-icon
			>{{ notificationModel.title }}
		</div>
		<div>{{ notification.description }}</div>
		<div class="text-caption">{{ notification.details }}</div>
		<div class="mt-4">
			<v-chip
				:color="notificationModel.color"
				small
				outlined
				class="font-weight-bold mt-2"
				>{{ notificationModel.type }}</v-chip
			>
		</div>
	</v-snackbar>
</template>
<script>
import { mapState } from "vuex";

export default {
	computed: {
		...mapState({
			notification: (state) => state.interface.notification,
		}),
		notificationModel: {
			get() {
				return { ...this.notification };
			},
			set(nV) {},
		},
	},
	methods: {
		notificationClicked() {
			if (this.notification.link) {
				window.open(this.notification.link, "_blank");
			}
		},
	},
};
</script>
