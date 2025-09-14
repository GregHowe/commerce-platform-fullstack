<template>
	<div class="container">
		<h1>Editor Tests</h1>
		<p>Throw your random editors in here to make sure they work.</p>
		<hr />

		<v-card>
			<v-card-title>EditorColor</v-card-title>
			<v-card-text>
				<div>
					<label>Color Value:</label>
					<EditorColor v-model="colorValue" />
				</div>
				<div><br />value: {{ colorValue }}</div>
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-title>EditorContent</v-card-title>
			<v-card-text>
				<EditorRichText v-model="contentValue" />
				<textarea
					v-model="contentValue"
					readonly
					style="width: 100%"
				/>
				<small>
					ctrl-b, ctrl-i both work to bold/emphasize, as well as other
					common shortcuts
				</small>
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-title>EditorInteger</v-card-title>
			<v-card-text>
				<div>
					<label>Integer Value:</label>
					<EditorInteger
						v-model="integerValue"
						:clearable="clearableIntegerValue"
					/>
				</div>
				<div>
					value: {{ integerValue }}, type: {{ typeof integerValue }}
				</div>
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-title>EditorSelect</v-card-title>
			<v-card-text>
				<EditorSelect
					v-model="selectValue"
					:items="selectItems"
				/>
				<br />value:
				{{ selectValue }}
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-title>EditorBoolean</v-card-title>
			<v-card-text>
				<EditorBoolean v-model="booleanValue" />
				<br />value:
				{{ booleanValue }}
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-title>EditorString</v-card-title>
			<v-card-text>
				<div>
					<label>Max Chars:</label>
					<br /><input
						v-model="maxlengthValue"
						type="number"
						value="12"
					/>
				</div>
				<div>
					<label>Clearable attr:</label>
					<br /><input
						v-model="clearableValue"
						type="checkbox"
					/>
				</div>
				<div>
					<label>String Value:</label>
					<EditorString
						v-model.trim="stringValue"
						:maxlength="maxlengthValue"
						:clearable="clearableValue"
					/>
					value: {{ stringValue }}, length: {{ stringValue.length }},
					type:
					{{ typeof stringValue }}
				</div>
			</v-card-text>
			<v-card-title>EditorRadio</v-card-title>
			<v-card-text>
				<EditorRadio
					v-model="radioValue"
					:items="radioItems"
					:mandatory="true"
				/>
				<br />
				value: {{ radioValue }}
			</v-card-text>
			<v-card-title>
				ForgeChip Forged from the Fires of Mount Ruffles
			</v-card-title>
		</v-card>
		<v-card>
			<v-card-text>
				<ForgeChip
					v-if="chip"
					:color="chip.color"
					:text-color="chip.textColor"
					:name="chip.label"
					:icon="chip.icon"
					close
					@click:close="chip = null"
				>
				</ForgeChip>
			</v-card-text>
			<v-card-text>
				<ForgeChipGroup
					:chips="chipGroup"
					:mandatory="mandatory"
					:multiple="multiple"
					@input="updateSomething"
				/>
			</v-card-text>
		</v-card>
		<v-card>
			<v-card-title>Forge Button</v-card-title>
			<v-card-text>
				<ForgeButton
					text
					:color="button.color"
				>
					Button Label
				</ForgeButton>
			</v-card-text>
			<v-card-title>Forge Button Group</v-card-title>
			<v-card-text>
				<ForgeButtonGroup
					v-model="buttonValue"
					:buttons="buttons"
					:mandatory="false"
					:multiple="true"
				>
				</ForgeButtonGroup>
				v-model toggle value: {{ buttonValue }}
			</v-card-text>
		</v-card>

		<v-card class="pa-16">
			<v-card-title>Editor Slider</v-card-title>
			<EditorSlider
				v-model="sliderValue"
				:min="5"
				:max="50"
				step="10"
				hint="i'm a hint"
				persistent-hint
				:append-icon="slider.appendIcon"
				:prepend-icon="slider.prependIcon"
				:prepend-click="updateSomething"
			>
			</EditorSlider>
			value: {{ sliderValue }}, type:
			{{ typeof sliderValue }}
		</v-card>
		<v-card>
			<v-card-title>Editor Address </v-card-title>
			<v-card-text>
				<EditorAddress v-model="address" />
				Address: {{ address.line1 }}
				{{ address.line2 }}
				City, State, Zip: {{ address.city }} {{ address.state }}
				{{ address.zip }}
			</v-card-text>
		</v-card>
		<v-card class="">
			<v-card-text>
				<v-card-title>Editor Phone </v-card-title>
				<EditorPhone v-model="phone" />
				value: {{ phone }}
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-text>
				<v-card-title>Email Editor</v-card-title>
				<EditorEmail v-model="email" />
				value: {{ email }}
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-text>
				<v-card-title>Editor URL </v-card-title>
				<EditorUrl v-model="url" />
				value: {{ url }}
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-text>
				<v-card-title>Editor Icon</v-card-title>
				<EditorIcon
					v-model="iconValue"
					:mandatory="false"
				/>
				value: {{ iconValue }}
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-text>
				<v-card-title>Editor Domain</v-card-title>
				<EditorDomain v-model="domainValue" />
				value: {{ domainValue }}
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-text>
				<v-card-title>File Editor</v-card-title>
				<EditorFile
					v-model="fileValue"
					hint="test passing in a hint"
				/>
				value: {{ fileValue ? `${fileValue.slice(0, 100)}...` : "" }}
			</v-card-text>
		</v-card>

		<v-card>
			<v-card-text>
				<v-card-title>TEST NYL LEAD FORM</v-card-title>
				<ForgeButton
					text
					@click="sendTestLeadForm"
				>
					Submit Test
				</ForgeButton>
				<ForgeButton
					text
					:disabled="!leadFormResponse"
					@click="clearTestLeadForm"
				>
					Clear Test
				</ForgeButton>
				<textarea
					v-if="leadFormResponse"
					v-model="leadFormResponse"
				/>
			</v-card-text>
		</v-card>
	</div>
</template>

<script>
import axios from "axios";
export default {
	name: "EditorTest",
	layout: "headeronly",
	data: () => {
		return {
			leadFormResponse: null,
			siteList: [],
			contentValue: `<h2>This is some H2 content.</h2><p>This is some paragraph content.</p>`,
			clearableValue: false,
			maxlengthValue: 12,
			stringValue: "something",
			colorValue: null,
			selectItems: [
				{
					value: "item1",
					text: "Item #1",
				},
				{
					value: "item2",
					text: "Item #2",
				},
				{
					value: "item3",
					text: "Item #3",
				},
			],
			selectValue: null,
			booleanValue: false,
			integerValue: 0,
			clearableIntegerValue: false,
			radioItems: [
				{
					value: "item1",
					label: "Item #1",
				},
				{
					value: "item2",
					label: "Item #2",
				},
				{
					value: "item3",
					label: "Item #3",
				},
			],
			radioValue: "",
			chip: {
				label: "ForgeChip",
				color: "red",
				textColor: "white",
				icon: {
					name: "mdi-bomb",
					color: "black",
				},
			},
			chipGroup: [
				{
					value: "bomb",
					label: "Bomb",
					color: "red",
					textColor: "white",
					icon: {
						name: "mdi-bomb",
						color: "black",
					},
				},
				{
					value: "airplane",
					label: "Airplane",
					color: "green",
					textColor: "white",
					icon: {
						name: "mdi-airplane",
						color: "white",
					},
				},
				{
					value: "anchor",
					label: "Anchor",
					color: "blue",
					textColor: "white",
					icon: {
						name: "mdi-anchor",
						color: "white",
					},
				},
				{
					value: "basketball",
					label: "Basketball",
					color: "purple",
					textColor: "yellow",
					icon: {
						name: "mdi-basketball",
						color: "green",
					},
				},
				{
					value: "duck",
					label: "Duck",
					color: "orange",
					textColor: "white",
					icon: {
						name: "mdi-duck",
						color: "white",
					},
				},
			],
			mandatory: false,
			multiple: false,
			button: {
				color: "primary",
			},
			sliderValue: 0,
			slider: {
				prependIcon: "mdi-minus",
				appendIcon: "mdi-plus",
			},
			buttonValue: [],
			buttons: [
				{ id: 1, value: "1", icon: "mdi-plus" },
				{ id: 2, value: "2", icon: "mdi-plus" },
				{ id: 3, value: "3", icon: "mdi-plus" },
			],
			address: {},
			phone: "",
			email: null,
			url: "",
			iconValue: "",
			domainValue: "",
			fileValue: null,
		};
	},
	methods: {
		async sendTestLeadForm() {
			const response = await axios.post(
				"https://mdl.f92clt.ws.newyorklife.com/PRO-MS/clt-leads-pxy/api/insertLeads",
				{
					firstName: "Terry",
					lastName: "Cloth",
				}
			);
			this.leadFormResponse = JSON.stringify(response);
		},
		clearTestLeadForm() {
			this.leadFormResponse = null;
		},
		updateSomething(event) {
			alert(`i emit button click ${event || ""}`);
		},
	},
};
</script>
