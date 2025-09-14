<template>
	<div class="editor editor-address">
		<EditorString
			v-model="valueLine1"
			label="Street Address"
		/>
		<EditorString
			v-model="valueLine2"
			label="Apartment / Suite Number"
		/>
		<div class="state-line">
			<EditorString
				v-model="valueCity"
				label="City"
			/>
			<EditorSelect
				v-model="valueState"
				:items="items"
				label="State"
				item-text="value"
			/>

			<EditorString
				v-model="valueZip"
				label="Zip Code"
				maxlength="11"
				hint="Format: 12345 or 12345-6789"
				persistent-hint
				rules="onlyNums"
				provider-name="Zip Code"
			/>
		</div>
	</div>
</template>

<script>
export default {
	name: "EditorAddress",
	inheritAttrs: false,
	props: {
		value: {
			type: Object,
			default: () => {
				return {};
			},
		},
	},
	data() {
		return {
			items: [
				{ value: "AL", text: "Alabama" },
				{ value: "AK", text: "Alaska" },
				{ value: "AZ", text: "Arizona" },
				{ value: "AR", text: "Arkansas" },
				{ value: "CA", text: "California" },
				{ value: "CO", text: "Colorado" },
				{ value: "CT", text: "Connecticut" },
				{ value: "DE", text: "Delaware" },
				{ value: "FL", text: "Florida" },
				{ value: "GA", text: "Georgia" },
				{ value: "HI", text: "Hawaii" },
				{ value: "ID", text: "Idaho" },
				{ value: "IL", text: "Illinois" },
				{ value: "IN", text: "Indiana" },
				{ value: "IA", text: "Iowa" },
				{ value: "KS", text: "Kansas" },
				{ value: "KY", text: "Kentucky" },
				{ value: "LA", text: "Louisiana" },
				{ value: "ME", text: "Maine" },
				{ value: "MD", text: "Maryland" },
				{ value: "MA", text: "Massachusetts" },
				{ value: "MI", text: "Michigan" },
				{ value: "MN", text: "Minnesota" },
				{ value: "MS", text: "Mississippi" },
				{ value: "MO", text: "Missouri" },
				{ value: "MT", text: "Montana" },
				{ value: "NE", text: "Nebraska" },
				{ value: "NV", text: "Nevada" },
				{ value: "NH", text: "New Hampshire" },
				{ value: "NJ", text: "New Jersey" },
				{ value: "NM", text: "New Mexico" },
				{ value: "NY", text: "New York" },
				{ value: "NC", text: "North Carolina" },
				{ value: "ND", text: "North Dakota" },
				{ value: "OH", text: "Ohio" },
				{ value: "OK", text: "Oklahoma" },
				{ value: "OR", text: "Oregon" },
				{ value: "PA", text: "Pennsylvania" },
				{ value: "RI", text: "Rhode Island" },
				{ value: "SC", text: "South Carolina" },
				{ value: "SD", text: "South Dakota" },
				{ value: "TN", text: "Tennessee" },
				{ value: "TX", text: "Texas" },
				{ value: "UT", text: "Utah" },
				{ value: "VT", text: "Vermont" },
				{ value: "VA", text: "Virginia" },
				{ value: "WA", text: "Washington" },
				{ value: "WV", text: "West Virginia" },
				{ value: "WI", text: "Wisconsin" },
				{ value: "WY", text: "Wyoming" },
			],
		};
	},
	computed: {
		valueLine1: {
			get() {
				return this.value?.line1;
			},
			set(inputValue) {
				this.emitInput({ ...this.value, ...{ line1: inputValue } });
			},
		},
		valueLine2: {
			get() {
				return this.value?.line2;
			},
			set(inputValue) {
				this.emitInput({ ...this.value, ...{ line2: inputValue } });
			},
		},
		valueCity: {
			get() {
				return this.value?.city;
			},
			set(inputValue) {
				this.emitInput({ ...this.value, ...{ city: inputValue } });
			},
		},
		valueState: {
			get() {
				return this.value?.state;
			},
			set(inputValue) {
				this.emitInput({ ...this.value, ...{ state: inputValue } });
			},
		},
		valueZip: {
			get() {
				return this.value?.zip;
			},
			set(inputValue) {
				this.emitInput({ ...this.value, ...{ zip: inputValue } });
			},
		},
	},
	methods: {
		emitInput(inputValues) {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}
			this.$emit("input", inputValues);
			return true;
		},
	},
};
</script>

<style lang="scss">
.state-line {
	display: flex;
}
</style>
