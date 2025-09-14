import "@testing-library/jest-dom";
import EditorDate from "~/components/editor/EditorDate";
import Vuetify from "vuetify";
import { render } from "@testing-library/vue";

const renderComponent = (propsData = {}) => {
	return render(EditorDate, {
		vuetify: new Vuetify(),
		propsData: {
			value: "",
			...propsData,
		},
	});
};

describe("EditorDate", () => {
	it("exists", () => {
		renderComponent({});
	});
});
