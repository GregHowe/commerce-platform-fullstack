<template>
	<v-container
		fluid
		class="px-0"
	>
		<v-row class="px-0">
			<v-col
				cols="12"
				md="6"
			>
				<h2>Filters</h2>
				<v-text-field
					dense
					class="mt-4"
					label="Search Name"
					autocomplete="search"
					full-width
					filled
					outlined
					@input="$emit('searchChange', $event)"
				/>
			</v-col>
			<v-col
				cols="12"
				md="3"
			>
				<h2>Status</h2>
				<v-checkbox
					v-model="siteStatus"
					class="d-inline-block mr-2"
					label="Live"
					color="success"
					value="0"
					hide-details
				></v-checkbox>
				<v-checkbox
					v-model="siteStatus"
					class="d-inline-block mr-2"
					label="In Review"
					color="warning"
					value="1"
					hide-details
				></v-checkbox>
				<v-checkbox
					v-model="siteStatus"
					class="d-inline-block mr-2"
					label="Changes Required"
					color="error"
					value="2"
					hide-details
				></v-checkbox>
			</v-col>
			<v-col
				cols="12"
				md="3"
			>
				<h2>View</h2>
				<v-btn-toggle
					v-model="viewtype"
					class="mt-2"
					tile
					dense
					flat
					group
					@change="$emit('changeViewType', $event)"
				>
					<v-btn
						small
						icon
						text
						elevation="0"
					>
						<v-icon>mdi-view-grid</v-icon>
					</v-btn>

					<v-btn
						small
						icon
						elevation="0"
					>
						<v-icon>mdi-format-list-bulleted</v-icon>
					</v-btn>
				</v-btn-toggle>
			</v-col>
		</v-row>
	</v-container>
</template>
<script>
export default {
	data() {
		return {
			viewtype: 0,
			siteStatus: [0, 1, 2],
		};
	},
};
</script>
