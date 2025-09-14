<script>
import { mapGetters } from "vuex";

export default {
	data() {
		return {
			showDialog: false,
		};
	},
	computed: {
		...mapGetters({
			firstTimeUser: "user/firstTimeUser",
			firstName: "user/firstName",
		}),
	},
	mounted() {
		this.firstTimeUser ? (this.showDialog = true) : "";
	},
};
</script>
<template>
	<v-dialog
		v-model="showDialog"
		persistent
		transition="fab-transition"
		max-width="1000"
	>
		<v-card>
			<v-container fluid>
				<v-row class="pa-12">
					<v-col cols="12">
						<v-card
							class="grey lighten-2 py-8"
							outlined
						>
							<v-card-text class="text-center">
								<v-icon
									size="100"
									color="black"
									>mdi-play-circle-outline</v-icon
								>
								<v-card-title
									class="mt-6 justify-center font-weight-bold"
								>
									Welcome to your new site!
								</v-card-title>
								<v-card-subtitle
									>Let's show you all the things you can
									do.</v-card-subtitle
								>
							</v-card-text>
						</v-card>
					</v-col>
					<v-col
						cols="12"
						lg="6"
					>
						<v-card
							class="grey lighten-3"
							outlined
						>
							<v-card-text class="text-center">
								<v-card-title
									class="justify-center font-weight-bold"
								>
									Questions?
								</v-card-title>
								<v-card-subtitle
									>Need help getting started? Visit our help
									guides.</v-card-subtitle
								>
								<ForgeButton text>
									See Help Guides
									<v-icon>mdi-chevron-right</v-icon>
								</ForgeButton>
							</v-card-text>
						</v-card>
					</v-col>
					<v-col
						cols="12"
						lg="6"
					>
						<v-card
							class="grey lighten-3"
							outlined
						>
							<v-card-text class="text-center">
								<v-card-title
									class="justify-center font-weight-bold"
								>
									Need Help?
								</v-card-title>
								<v-card-subtitle
									>Visit our training content to help get you
									started.</v-card-subtitle
								>
								<ForgeButton text>
									Training
									<v-icon>mdi-chevron-right</v-icon>
								</ForgeButton>
							</v-card-text>
						</v-card>
					</v-col>
				</v-row>
			</v-container>

			<v-card-actions class="justify-end">
				<v-btn
					text
					@click="showDialog = false"
					>Close</v-btn
				>
			</v-card-actions>
		</v-card>
	</v-dialog>
</template>
