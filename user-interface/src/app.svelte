<script>
  import Button from "./atoms/button.svelte";
  import TextInput from "./atoms/forms/text-input.svelte";
  import Col from "./atoms/grid/col.svelte";
  import Row from "./atoms/grid/row.svelte";
</script>

<div>
  <Row>
    <Col>
      <TextInput name="TestInput">Test Input</TextInput>
    </Col>
    <Col>
      <Button type="submit">Submit</Button>
    </Col>
  </Row>
</div>

<style>
  div {
    container-type: size;

    --colours-body-background: #fcfcfc;
    --colours-body-foreground: #333;

    --colours-highlight-background: #f2f2f2;
    --colours-highlight-foreground: #333;

    --colours-theme-primary-background: #b100dd;
    --colours-theme-primary-foreground: #fff;

    --colours-theme-secondary-background: #00f5f1;
    --colours-theme-secondary-foreground: #222;

    --colours-status-warning-background: #ffdd00;
    --colours-status-warning-foreground: #000;

    --colours-status-error-background: #ff0000;
    --colours-status-error-foreground: #fff;

    --font-family: sans-serif;
    --font-size-body: 1rem;
  }

  @media (prefers-color-scheme: dark) {
    div {
      --colours-body-background: #434343;
      --colours-body-foreground: #ececec;

      --colours-highlight-background: #1b1b1b;
      --colours-highlight-foreground: #ffffff;
    }
  }
</style>
