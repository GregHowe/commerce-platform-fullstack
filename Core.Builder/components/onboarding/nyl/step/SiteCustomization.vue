<template>
	<v-card
		min-height="100%"
		outlined
	>
		<v-card-text>
			<v-combobox
				v-model="select"
				:items="pageItems"
				label="Prebuilt Pages"
				multiple
				outlined
				dense
			></v-combobox>

			<v-combobox
				v-model="select"
				:items="calculatorItems"
				label="Calculator List"
				multiple
				outlined
				dense
			></v-combobox>
		</v-card-text>
	</v-card>
</template>
<script>
export default {
	data() {
		return {
			select: "",
			calculatorItems: [
				"Life Insurance Benefits Calculator",
				"Annuity Tax Advantages Calculator",
				"Retirement Savings Calculator",
				"Living Expenses Calculator",
				"Savings in Retirement Calculator",
				"Funeral Costs and Final Expenses Calculator",
				"Savings Goals Calculator",
				"College Savings Calculator",
				"401(k) Retirement Income Calculator",
				"What is the Cost of Long Term Care",
			],
			pageItems: [
				"About Eagle Strategies",
				"About New York Life",
				"Advanced Planning Resources",
				"Business Solutions (Agents, Eagle)",
				"Business Solutions (DBA, GO)",
				"Life Stages (Agents, Eagle)",
				"Life Stages (DBA, GO)",
				"Investment Advisory Services",
				"Products (Agent Version)",
				"Products (Eagle Adviser Version)",
				"Products (Registered Rep Version)",
				"Retirement Solutions for Businesses",
				"The Nautilus Group",
				"AARP Life Insurance and Long-Term Care Options from New York Life (Eagle & DBA Version)",
				"AARP Life Insurance and Long-Term Care Options from New York Life (for California DBA and Eagle)",
				"AARP Life Insurance and Long-Term Care Options from New York Life (for California NYL Agents)",
				"AARP Life Insurance and Long-Term Care Options from New York Life (NYL Branded Version)",
				"AARP Life Insurance and Long-Term Care Options from New York Life (Florida version)",
				"AARP Life Insurance Program from NYL (DBA, Eagle)",
				"AARP Life Insurance Program from NYL (Agent)",
				"Services (Agent)",
				"Services (DBA)",
				"Services (Eagle)",
				"Services (GO)",
				"My Background (Agent)",
				"My Background (DBA)",
				"My Background (Eagle)",
				"Home Page - Young Families Focus (Agent, DBA, Eagle)",
				"Home Page - Retirement Focus (Agent, DBA, Eagle)",
				"Home Page - Generic (Agent, DBA, Eagle)",
				"Home Page - Pre-retirement Focus (Agent, DBA, Eagle)",
				"Home Page - Generic (Eagle, Reg Rep)",
				"Home Page - Generic 2 (Agent)",
				"Home Page - About Our GO (GO)",
				"Home Page - Recruiting (GO)",
				"Home Page - Genric GO (GO)",
				"Tax Resources (Agent, DBA, Reg Rep)",
				"Contact Us (Agent, DBA, Eagle, Reg Rep)",
				"Contact Us (GO)",
				"Becoming an Agent (GO)",
				"For Professional Advisors (GO)",
				"Career Opportunity - SalesRep (GO)",
				"Career Opportunity - AdminAsst (GO)",
				"Career Opportunity - Intern (GO)",
				"Career Opportunity - FinancePro (GO)",
				"Career Opportunity - RecruitementCoordinator (GO),",
			],
		};
	},
};
</script>
