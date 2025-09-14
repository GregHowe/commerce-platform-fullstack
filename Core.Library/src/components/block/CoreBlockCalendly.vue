<template>
	<div>
		<CoreBlock
			v-if="getDisplayType === 'Button'"
			:settings="primaryBtn"
			class="primary-button"
		/>
		<a
			v-if="getDisplayType === 'Link'"
			class="calendly__link"
			@click="showPopUp"
		>
			{{ buttonText }}
		</a>
	</div>
</template>

<script>
export default {
	name: "CoreBlockCalendly",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		calendlyUrl() {
			let eventType = this.settings.eventType;
			if (eventType == "Custom Event") {
				return `https://calendly.com/${this.settings.calendlyId}/${this.settings.customEvent}?utm_campaign=marketing&utm_source=agentwebsites`;
			} else {
				eventType = eventType.toLowerCase().replace(/\s/g, "");
				return `https://calendly.com/${this.settings.calendlyId}/${eventType}?utm_campaign=marketing&utm_source=agentwebsites`;
			}
		},
		buttonText() {
			return this.settings.buttonText;
		},
		getDisplayType() {
			return this.settings.displayType;
		},
		primaryBtn() {
			return (
				{
					type: "button",
					label: this.buttonText,
					newTab: this.settings.primaryBtnNewTab,
					event: this.showPopUp,
				} || null
			);
		},
	},

	methods: {
		showPopUp() {
			if (process.client) {
				window.Calendly.showPopupWidget(this.calendlyUrl);
			}
			return false;
		},
	},
};
</script>

<style scoped>
.calendly__button {
	cursor: pointer;
	font-family: "Montserrat";
	font-style: normal;
	font-weight: 600;
	font-size: 14px;
	padding: 10px 16px;
	/* color: var(--_core__button_primary-color);
	background-color: var(--_core__button_primary-background); */
	background: #000000;
	color: #ffffff;
	text-decoration: none;
	border: 1px solid #ffffff;
	border-radius: 3.2977px;
}

.calendly__link {
	cursor: pointer;
}
</style>
