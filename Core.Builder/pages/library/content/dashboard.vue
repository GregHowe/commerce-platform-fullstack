<template>
	<div>
		<ContentLibraryHeader
			:heading="`Hi, ${fullName}`"
			sub-heading="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed
					do eiusmod tempor incididunt ut labore et dolore magna
					aliqua."
		/>
		<v-row>
			<v-col
				cols="12"
				md="4"
			>
				<v-card
					class="secondary rounded-lg d-flex flex-column justify-end"
					flat
				>
					<v-card-subtitle
						class="text-center mb-14 font-weight-medium"
					>
						Assets waiting to be submitted to the Content Library
					</v-card-subtitle>
					<v-card-title> Your Saved Assets </v-card-title>
				</v-card>
			</v-col>
			<v-col
				cols="12"
				md="4"
			>
				<v-card
					class="secondary rounded-lg d-flex flex-column justify-end"
					flat
				>
					<v-card-subtitle
						class="text-center mb-14 font-weight-medium"
					>
						Assets you have recently added to the Content Library
					</v-card-subtitle>
					<v-card-title> Recently Added </v-card-title>
				</v-card>
			</v-col>
		</v-row>
		<v-row>
			<v-col
				cols="12"
				md="4"
			>
				<v-card
					class="secondary rounded-lg d-flex flex-column justify-end"
					flat
					:to="{ name: 'library-content-compliance' }"
				>
					<v-card-subtitle
						class="text-center mb-14 font-weight-medium"
					>
						Most recent Compliance submissions and status
					</v-card-subtitle>
					<v-card-title>Compliance </v-card-title>
				</v-card>
			</v-col>
			<v-col
				cols="12"
				md="4"
			>
				<v-card
					class="secondary rounded-lg d-flex flex-column justify-end"
					flat
					:to="{ name: 'library-content' }"
				>
					<v-card-subtitle
						class="text-center mb-14 font-weight-medium"
					>
						Newest Items
					</v-card-subtitle>
					<v-card-title> Content Library </v-card-title>
				</v-card>
			</v-col>
		</v-row>
	</div>
</template>
<script>
import { mapGetters } from "vuex";
export default {
	layout: "default",
	computed: {
		...mapGetters({
			fullName: "user/fullName",
		}),
	},
};
</script>
