<template>
	<v-card
		min-height="100%"
		outlined
	>
		<v-card-text>
			<v-select
				:items="templateType"
				label="Template Type"
				outlined
			></v-select>
			<v-radio-group
				v-model="radios"
				class="mb-4"
			>
				<template #label>
					<div>Is this agent part of a DBA?</div>
				</template>
				<v-radio value="yes">
					<template #label>
						<div>Yes</div>
					</template>
				</v-radio>
				<v-radio value="no">
					<template #label>
						<div>No</div>
					</template>
				</v-radio>
			</v-radio-group>
			<v-text-field
				v-if="radios === 'yes'"
				label="DBA Display Name"
				outlined
			></v-text-field>
		</v-card-text>
	</v-card>
</template>
<script>
export default {
	data() {
		return {
			templateType: [
				"Retail",
				"Flex",
				"Eagle",
				"GO",
				"NYL Brand",
				"Custom",
			],
			radios: "yes",
		};
	},
};
</script>
