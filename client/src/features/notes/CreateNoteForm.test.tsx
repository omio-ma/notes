import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import CreateNoteForm from "./CreateNoteForm";
import * as createHook from "../../lib/hooks/notes/useCreateNote";

const mockNavigate = jest.fn();
const mockMutate = jest.fn((_, { onSuccess }) => onSuccess?.());

jest.mock("react-router", () => ({
  useNavigate: () => mockNavigate
}));

jest.mock("../../lib/hooks/notes/useCreateNote");
jest.mock("../../lib/agent");

describe("CreateNoteForm", () => {
  beforeEach(() => {
    (createHook.useCreateNote as jest.Mock).mockReturnValue({
      mutate: mockMutate
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("submits form and calls mutate with note data", async () => {
    render(<CreateNoteForm />);

    const titleInput = screen.getByTestId("create-note-form-title");
    const contentTextarea = screen.getByTestId("create-note-form-content");

    fireEvent.change(titleInput, { target: { value: "New Title" } });
    fireEvent.change(contentTextarea, { target: { value: "New Content" } });

    fireEvent.click(screen.getByTestId("create-note-form-save-button"));

    await waitFor(() => {
      expect(mockMutate).toHaveBeenCalledWith(
        {
          title: "New Title",
          content: "New Content"
        },
        expect.objectContaining({
          onSuccess: expect.any(Function)
        })
      );

      expect(mockNavigate).toHaveBeenCalledWith("/");
    });
  });

  it("navigates back when Back button is clicked", () => {
    render(<CreateNoteForm />);

    fireEvent.click(screen.getByTestId("create-note-form-back-button"));

    expect(mockNavigate).toHaveBeenCalledWith("/");
  });
});
