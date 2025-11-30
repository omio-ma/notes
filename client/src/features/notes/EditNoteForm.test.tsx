import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import EditNoteForm from "./EditNoteForm";
import type { Note } from "../../lib/types";
import * as notesHook from "../../lib/hooks/notes/useNotes";
import * as updateHook from "../../lib/hooks/notes/useUpdateNote";

jest.mock("@auth0/auth0-react", () => ({
  useAuth0: () => ({
    isAuthenticated: true,
    user: { sub: "test-user" },
    getAccessTokenSilently: jest.fn()
  })
}));

jest.mock("../../lib/hooks/notes/useNotes");
jest.mock("../../lib/hooks/notes/useUpdateNote");
jest.mock("../../lib/agent");

const mockNavigate = jest.fn();
const mockUseParams = jest.fn();

jest.mock("react-router-dom", () => ({
  useNavigate: () => mockNavigate,
  useParams: () => mockUseParams()
}));

describe("EditNoteForm", () => {
  const mockNote: Note = {
    id: 1,
    title: "Original Title",
    content: "Original Content",
    createdAt: new Date().toISOString()
  };

  beforeEach(() => {
    (notesHook.useNotes as jest.Mock).mockReturnValue({
      data: [mockNote]
    });

    (updateHook.useUpdateNote as jest.Mock).mockReturnValue({
      mutate: jest.fn((_, { onSuccess }) => onSuccess?.())
    });

    mockUseParams.mockReturnValue({ id: "1" });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("updates input fields and calls mutate on save", async () => {
    const mockMutate = jest.fn((_, { onSuccess }) => onSuccess?.());

    (updateHook.useUpdateNote as jest.Mock).mockReturnValue({
      mutate: mockMutate
    });

    render(<EditNoteForm />);

    const titleInput = screen.getByTestId("note-form-title");
    const contentTextarea = screen.getByTestId("note-form-content");

    fireEvent.change(titleInput, { target: { value: "Updated Title" } });
    fireEvent.change(contentTextarea, { target: { value: "Updated Content" } });

    fireEvent.click(screen.getByTestId("note-form-save-button"));

    await waitFor(() => {
      expect(mockMutate).toHaveBeenCalledWith(
        {
          id: 1,
          data: {
            title: "Updated Title",
            content: "Updated Content"
          }
        },
        expect.objectContaining({
          onSuccess: expect.any(Function)
        })
      );

      expect(mockNavigate).toHaveBeenCalledWith("/");
    });
  });

  it("calls navigate on back button click", () => {
    render(<EditNoteForm />);

    fireEvent.click(screen.getByTestId("note-form-back-button"));

    expect(mockNavigate).toHaveBeenCalledWith("/");
  });

  it("shows 'Note not found' if ID doesn't match", () => {
    mockUseParams.mockReturnValue({ id: "99" });
    (notesHook.useNotes as jest.Mock).mockReturnValue({ data: [mockNote] });

    render(<EditNoteForm />);

    expect(screen.getByText("Note not found")).toBeDefined();
  });
});
