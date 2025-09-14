<template>
	<v-toolbar
		dense
		height="40px"
		elevation="0"
	>
		<div class="content">
			<v-text-field
				prepend-inner-icon="mdi-magnify"
				label="Search"
				background-color="#00000000"
				dense
				solo
				flat
				@change="$emit('changeSearch', $event)"
			/>
			<v-spacer />
			<v-select
				v-model="selectedSort"
				label="Sort"
				dense
				solo
				flat
				color="primary"
				background-color="#00000000"
				style="max-width: 200px"
				:items="sortOptions"
				@change="$emit('changeSortType', $event)"
			/>
			<v-btn-toggle
				v-model="viewtype"
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
		</div>
	</v-toolbar>
</template>
<script>
export default {
	data() {
		return {
			viewtype: 0,
			selectedSort: "Newest",
			sortOptions: ["Newest", "Popular", "Last Edited"],
		};
	},
};
</script>
<style
	lang="scss"
	scoped
>
.content {
	display: flex;
	width: 100%;
	height: 40px !important;
	border-top: solid 1px grey;
	border-bottom: solid 1px grey;
}
</style>
