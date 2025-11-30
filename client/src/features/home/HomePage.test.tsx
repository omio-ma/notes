import type { UseQueryResult } from "@tanstack/react-query";
import { render, screen } from "@testing-library/react";
import * as useNotesModule from "../../lib/hooks/notes/useNotes";
import type { Note } from "../../lib/types";
import HomePage from "./HomePage";

jest.mock("@auth0/auth0-react", () => ({
  useAuth0: () => ({
    isAuthenticated: true,
    user: { sub: "test-user" },
    getAccessTokenSilently: jest.fn()
  })
}));

jest.mock("@mui/material", () => ({
  ...jest.requireActual("@mui/material"),
  CircularProgress: () => <div data-testid="loader">Loading...</div>
}));

jest.mock("../notes/NotesList", () => () => (
  <div data-testid="mock-notes-list">NotesList</div>
));

jest.mock("../../lib/hooks/notes/useNotes");
const useNotesMock = useNotesModule as jest.Mocked<typeof useNotesModule>;

jest.mock("../../lib/agent");

describe("HomePage", () => {
  it("shows loading state", () => {
    useNotesMock.useNotes.mockReturnValue({
      data: undefined,
      isLoading: true,
      error: null
    } as unknown as UseQueryResult<Note[]>);

    render(<HomePage />);
    expect(screen.getByTestId("loader")).toBeDefined();
  });

  it("shows error message", () => {
    useNotesMock.useNotes.mockReturnValue({
      data: undefined,
      isLoading: false,
      error: new Error("Fetch failed")
    } as unknown as UseQueryResult<Note[]>);

    render(<HomePage />);
    expect(screen.getByText(/Failed to load notes/i)).toBeDefined();
  });

  it("renders NotesList on success", () => {
    const mockNotes: Note[] = [
      {
        id: 1,
        title: "Note A",
        content: "A content",
        createdAt: new Date().toISOString()
      }
    ];

    useNotesMock.useNotes.mockReturnValue({
      data: mockNotes,
      isLoading: false,
      error: null
    } as unknown as UseQueryResult<Note[]>);

    render(<HomePage />);
    expect(screen.getByTestId("mock-notes-list")).toBeDefined();
  });
});
